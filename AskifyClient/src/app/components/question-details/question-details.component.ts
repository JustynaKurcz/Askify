import { Component, inject, Input, OnInit } from '@angular/core';
import { finalize, forkJoin, of } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { Answer, CreateAnswer, Question, QuestionService } from '../../services/question.service';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { DatePipe, NgForOf, NgIf } from '@angular/common';
import { CreateAnswerComponent } from '../create-answer/create-answer.component';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

interface AnswerWithAuthor extends Answer {
  authorName?: string;
  isEditing?: boolean;
  editContent?: string;
  isSaving?: boolean;
}

interface Tag {
  id: number;
  name: string;
  displayName: string;
}

@Component({
  selector: 'app-question-details',
  templateUrl: './question-details.component.html',
  standalone: true,
  imports: [
    FormsModule,
    NgForOf,
    CreateAnswerComponent,
    NgIf,
    DatePipe,
    ProgressSpinnerModule,
    ToastModule
  ],
  providers: [MessageService],
  styleUrls: ['./question-details.component.css']
})
export class QuestionDetailsComponent implements OnInit {
  @Input() questionId!: string;

  private readonly questionService = inject(QuestionService);
  private readonly authService = inject(AuthService);
  private readonly messageService = inject(MessageService);

  question: Question | null = null;
  answers: AnswerWithAuthor[] = [];
  tags: Tag[] = [];
  authorQuestion = '';
  currentUserId = localStorage.getItem('userId');

  loading = true;
  error = false;
  showAnswerForm = false;
  isEditing = false;
  editQuestionData = {
    title: '',
    content: '',
    tag: 1
  };

  ngOnInit() {
    this.loadInitialData();
  }

  private loadInitialData() {
    this.loadTags();
    this.loadAllData();
  }

  private loadTags() {
    this.questionService.getTags().subscribe({
      next: (tags) => this.tags = tags,
      error: (error) => console.error('Error loading tags:', error)
    });
  }

  private loadAllData() {
    this.loading = true;
    this.error = false;

    this.questionService.getQuestion(this.questionId).pipe(
      switchMap(question => {
        this.question = question;
        return this.loadQuestionDetails(question);
      }),
      switchMap(({author, answers}) => {
        this.authorQuestion = author;
        return this.loadAnswerAuthors(answers);
      }),
      finalize(() => this.loading = false)
    ).subscribe({
      next: ({answers, authors}) => {
        this.answers = this.mapAnswersWithAuthors(answers, authors);
      },
      error: (err) => this.handleError(err)
    });
  }

  private loadQuestionDetails(question: Question) {
    return forkJoin({
      author: this.authService.getUserName(question.userId).pipe(
        catchError(() => of('Nieznany użytkownik'))
      ),
      answers: this.questionService.getQuestionAnswers(question.questionId)
    });
  }

  private loadAnswerAuthors(answers: Answer[]) {
    const authorRequests = answers.map(answer =>
      this.authService.getUserName(answer.userId).pipe(
        catchError(() => of('Nieznany użytkownik'))
      )
    );

    return forkJoin({
      answers: of(answers),
      authors: forkJoin(authorRequests)
    });
  }

  private mapAnswersWithAuthors(answers: Answer[], authors: string[]) {
    return answers.map((answer, index) => ({
      ...answer,
      authorName: authors[index]
    }));
  }

  private handleError(err: any) {
    this.messageService.add({
      severity: 'error',
      summary: 'Błąd',
      detail: err.reason
    });
    this.error = true;
  }

  startEditingQuestion() {
    this.isEditing = true;
    this.editQuestionData = {
      title: this.question!.title,
      content: this.question!.content,
      tag: +this.tags.find(tag => tag.displayName === this.question!.tag)?.id!
    };
  }

  cancelEditingQuestion() {
    this.isEditing = false;
  }

  saveQuestionEdit() {
    if (!this.question?.questionId) return;

    this.loading = true;
    const data = {
      ...this.editQuestionData,
      tag: +this.editQuestionData.tag
    };

    this.questionService.updateQuestion(this.question.questionId, data)
      .pipe(finalize(() => this.loading = false))
      .subscribe({
        next: (updatedQuestion) => this.handleQuestionUpdateSuccess(updatedQuestion),
        error: (error) => this.handleQuestionUpdateError(error)
      });
  }

  private handleQuestionUpdateSuccess(updatedQuestion: Question) {
    this.question = updatedQuestion;
    this.isEditing = false;
    this.loadAllData();
  }

  private handleQuestionUpdateError(error: any) {
    const message = error?.error.code === 'question_cannot_be_changed'
      ? 'Nie można edytować pytania po upływie 30 minut od jego utworzenia.'
      : 'Wystąpił błąd podczas aktualizacji pytania.';

    this.messageService.add({
      severity: 'error',
      summary: 'Błąd',
      detail: message
    });
  }

  deleteQuestion() {
    if (!this.question?.questionId) return;

    this.questionService.deleteQuestion(this.question.questionId).subscribe({
      next: () => this.handleQuestionDeleteSuccess(),
      error: () => this.handleQuestionDeleteError()
    });
  }

  private handleQuestionDeleteSuccess() {
    this.messageService.add({
      severity: 'success',
      summary: 'Sukces',
      detail: 'Pytanie zostało usunięte'
    });
    window.location.reload();
  }

  private handleQuestionDeleteError() {
    this.messageService.add({
      severity: 'error',
      summary: 'Błąd',
      detail: 'Wystąpił błąd podczas usuwania pytania'
    });
  }

  addAnswer() {
    this.showAnswerForm = true;
  }

  handleAnswerSubmitted(data: any) {
    if (!this.question?.questionId) return;

    this.loading = true;
    const answer: CreateAnswer = { content: data.content };

    this.questionService.createAnswer(this.question.questionId, answer)
      .pipe(finalize(() => this.loading = false))
      .subscribe({
        next: () => {
          this.loadAllData();
          this.showAnswerForm = false;
        },
        error: () => this.showAnswerForm = false
      });
  }

  startEditing(answerId: string) {
    const answer = this.answers.find(a => a.answerId === answerId);
    if (answer) {
      answer.isEditing = true;
      answer.editContent = answer.content;
      answer.isSaving = false;
    }
  }

  cancelEditing(answer: AnswerWithAuthor) {
    answer.isEditing = false;
    answer.editContent = undefined;
  }

  saveEdit(answer: AnswerWithAuthor) {
    if (!answer.editContent?.trim()) return;

    answer.isSaving = true;
    const updateData = { content: answer.editContent.trim() };

    this.questionService.updateAnswer(this.question?.questionId!, answer.answerId, updateData)
      .pipe(finalize(() => answer.isSaving = false))
      .subscribe({
        next: () => {
          answer.content = updateData.content;
          answer.isEditing = false;
        },
        error: () => {}
      });
  }

  deleteAnswer(answerId: string) {
    this.loading = true;
    this.questionService.deleteAnswer(this.question?.questionId!, answerId)
      .pipe(finalize(() => this.loading = false))
      .subscribe({
        next: () => {
          this.answers = this.answers.filter(a => a.answerId !== answerId);
        },
        error: () => console.error('Błąd podczas usuwania odpowiedzi')
      });
  }
}
