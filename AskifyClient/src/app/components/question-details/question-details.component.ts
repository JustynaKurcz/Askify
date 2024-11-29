import {Component, Input, OnChanges, SimpleChanges} from '@angular/core';
import {SkeletonModule} from 'primeng/skeleton';
import {CardModule} from 'primeng/card';
import {DatePipe, NgForOf, NgIf} from '@angular/common';
import {DividerModule} from 'primeng/divider';
import {Answer, Question, QuestionService} from '../../services/question.service';
import {Button} from 'primeng/button';
import {MenuModule} from 'primeng/menu';
import {AuthService} from '../../services/auth.service';

interface AnswerWithAuthor extends Answer {
  authorName?: string;
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
    MenuModule
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
  constructor(private questionService: QuestionService, private authService : AuthService) {}



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
}
