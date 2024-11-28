import {Component, Input, OnChanges, OnInit, SimpleChanges} from '@angular/core';
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
export class QuestionDetailsComponent implements OnInit, OnChanges {
  @Input() question: Question | null = null;
  answers: AnswerWithAuthor[] = [];
  loading = false;
  currentUserId = localStorage.getItem('userId');

  constructor(private questionService: QuestionService, private authService : AuthService) {}

  ngOnInit() {
    if (this.question) {
      this.loadAnswers();
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['question'] && changes['question'].currentValue) {
      this.loadAnswers();
    }
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
