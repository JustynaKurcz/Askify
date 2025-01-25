import {Component, inject, Input, OnInit} from '@angular/core';
import {SkeletonModule} from 'primeng/skeleton';
import {CardModule} from 'primeng/card';
import {DatePipe, NgForOf, NgIf} from '@angular/common';
import {DividerModule} from 'primeng/divider';
import {Answer, CreateAnswer, Question, QuestionService} from '../../services/question.service';
import {MenuModule} from 'primeng/menu';
import {AuthService} from '../../services/auth.service';
import {PaginatorModule} from 'primeng/paginator';
import {InputTextareaModule} from 'primeng/inputtextarea';
import {ProgressSpinnerModule} from 'primeng/progressspinner';
import {forkJoin, of} from 'rxjs';
import {catchError, finalize, switchMap} from 'rxjs/operators';
import {Button} from 'primeng/button';
import {CreateAnswerComponent} from '../create-answer/create-answer.component';

interface AnswerWithAuthor extends Answer {
  authorName?: string;
  isEditing?: boolean;
  editContent?: string;
  isSaving?: boolean;
}

@Component({
  selector: 'app-question-details',
  standalone: true,
  imports: [
    SkeletonModule,
    CardModule,
    NgIf,
    DividerModule,
    MenuModule,
    PaginatorModule,
    InputTextareaModule,
    ProgressSpinnerModule,
    DatePipe,
    Button,
    CreateAnswerComponent,
    NgForOf
  ],
  templateUrl: './question-details.component.html',
  styleUrl: './question-details.component.css'
})
export class QuestionDetailsComponent implements OnInit {
  @Input() questionId!: string;

  private readonly questionService = inject(QuestionService);
  private readonly authService = inject(AuthService);

  question: Question | null = null;
  answers: AnswerWithAuthor[] = [];
  authorQuestion: string = '';
  loading = true;
  error = false;
  showAnswerForm = false;
  currentUserId = localStorage.getItem('userId');

  ngOnInit() {
    this.loadAllData();
  }

  private loadAllData() {
    this.loading = true;
    this.error = false;

    this.questionService.getQuestion(this.questionId).pipe(
      switchMap(question => {
        this.question = question;
        return forkJoin({
          author: this.authService.getUserName(question.userId).pipe(
            catchError(() => of('Nieznany użytkownik'))
          ),
          answers: this.questionService.getQuestionAnswers(question.questionId)
        });
      }),
      switchMap(({author, answers}) => {
        this.authorQuestion = author;
        const authorRequests = answers.map(answer =>
          this.authService.getUserName(answer.userId).pipe(
            catchError(() => of('Nieznany użytkownik'))
          )
        );
        return forkJoin({
          answers: of(answers),
          authors: forkJoin(authorRequests)
        });
      }),
      finalize(() => {
        this.loading = false;
      })
    ).subscribe({
      next: ({answers, authors}) => {
        this.answers = answers.map((answer, index) => ({
          ...answer,
          authorName: authors[index]
        }));
      },
      error: (err) => {
        console.error('Error loading data:', err);
        this.error = true;
      }
    });
  }

  addAnswer() {
    this.showAnswerForm = true;
  }

  handleAnswerSubmitted(data: any) {
    if (!this.question?.questionId) return;

    this.loading = true;
    const answer: CreateAnswer = {
      content: data.content,
    };

    this.questionService.createAnswer(this.question.questionId, answer).pipe(
      finalize(() => this.loading = false)
    ).subscribe({
      next: () => {
        this.loadAllData();
        this.showAnswerForm = false;
      },
      error: () => {
        this.showAnswerForm = false;
      }
    });
  }

  deleteAnswer(answerId: string) {
    this.loading = true;
    this.questionService.deleteAnswer(this.question?.questionId!, answerId).pipe(
      finalize(() => this.loading = false)
    ).subscribe({
      next: () => this.loadAllData(),
      error: () => console.error('Błąd podczas usuwania odpowiedzi')
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
    const updateData = {
      content: answer.editContent.trim()
    };

    this.questionService.updateAnswer(this.question?.questionId!, answer.answerId, updateData).pipe(
      finalize(() => answer.isSaving = false)
    ).subscribe({
      next: () => {
        answer.content = answer.editContent!;
        answer.isEditing = false;
      },
      error: () => {}
    });
  }
}
