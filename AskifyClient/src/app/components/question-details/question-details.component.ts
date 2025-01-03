import {Component, Input, OnChanges, SimpleChanges} from '@angular/core';
import {SkeletonModule} from 'primeng/skeleton';
import {CardModule} from 'primeng/card';
import {DatePipe, NgForOf, NgIf} from '@angular/common';
import {DividerModule} from 'primeng/divider';
import {Answer, CreateAnswer, Question, QuestionService} from '../../services/question.service';
import {Button} from 'primeng/button';
import {MenuModule} from 'primeng/menu';
import {AuthService} from '../../services/auth.service';
import {CreateAnswerComponent} from '../create-answer/create-answer.component';
import {PaginatorModule} from 'primeng/paginator';
import {InputTextareaModule} from 'primeng/inputtextarea';

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
    DatePipe,
    NgForOf,
    Button,
    MenuModule,
    CreateAnswerComponent,
    PaginatorModule,
    InputTextareaModule
  ],
  templateUrl: './question-details.component.html',
  styleUrl: './question-details.component.css'
})
export class QuestionDetailsComponent implements OnChanges {
  @Input() question: Question | null = null;
  answers: AnswerWithAuthor[] = [];
  loading = false;
  currentUserId = localStorage.getItem('userId');
  authorQuestion: string = '';
  showAnswerForm: boolean = false;

  constructor(private questionService: QuestionService, private authService: AuthService) {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['question'] && changes['question'].currentValue) {
      this.loadAnswers();
      this.loadAuthorQuestion();
    }
  }

  loadAuthorQuestion() {
    this.authService.getUserName(this.question?.userId!).subscribe({
      next: (userName) => {
        this.authorQuestion = userName;
      },
      error: () => {
        this.authorQuestion = 'Nieznany użytkownik';
      }
    });
  }

  loadAnswers() {
    if (!this.question) return;

    this.loading = true;
    this.questionService.getQuestionAnswers(this.question.questionId).subscribe({
      next: (data) => {
        this.answers = data;
        this.answers.forEach(answer => {
          this.authService.getUserName(answer.userId).subscribe({
            next: (userName) => {
              answer.authorName = userName;
            },
            error: () => {
              answer.authorName = 'Nieznany użytkownik';
            }
          });
        });
        this.loading = false;
      },
      error: (error) => {
        console.error('Błąd podczas ładowania odpowiedzi:', error);
        this.loading = false;
      }
    });
  }

  addAnswer() {
    this.showAnswerForm = true;
  }

  handleAnswerSubmitted(data: any) {
    if (!this.question?.questionId) return;

    const answer: CreateAnswer = {
      content: data.content,
    };

    this.questionService.createAnswer(this.question.questionId, answer).subscribe({
      next: () => {

        this.loadAnswers();
        this.showAnswerForm = false;
      },
      error: () => {
        this.showAnswerForm = false;
      }
    });
  }

  deleteAnswer(answerId: string) {
    this.questionService.deleteAnswer(this.question?.questionId!, answerId).subscribe({
      next: () => {
        this.loadAnswers();
      },
      error: () => {
        console.error('Błąd podczas usuwania odpowiedzi');
      }
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
    if (!answer.editContent?.trim()) {
      return;
    }

    answer.isSaving = true;
    const updateData = {
      content: answer.editContent.trim()
    };

    this.questionService.updateAnswer(this.question?.questionId!, answer.answerId, updateData).subscribe({
      next: () => {
        answer.content = answer.editContent!;
        answer.isEditing = false;
        answer.isSaving = false;
      },
      error: () => {
        answer.isSaving = false;
      }
    });
  }
}
