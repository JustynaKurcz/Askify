import {Component, OnInit} from '@angular/core';
import {DialogModule} from 'primeng/dialog';
import {Button} from 'primeng/button';
import {TableModule} from 'primeng/table';
import {MessageService} from 'primeng/api';
import {Question, QuestionService} from '../../services/question.service';
import {MenuModule} from 'primeng/menu';
import {CardModule} from 'primeng/card';
import {InputTextModule} from 'primeng/inputtext';
import {DividerModule} from 'primeng/divider';
import {TagModule} from 'primeng/tag';
import {SkeletonModule} from 'primeng/skeleton';
import {DatePipe, NgForOf} from '@angular/common';
import {QuestionDetailsComponent} from '../question-details/question-details.component';

@Component({
  selector: 'app-question-list',
  standalone: true,
  imports: [
    DialogModule,
    TableModule,
    MenuModule,
    CardModule,
    InputTextModule,
    DividerModule,
    TagModule,
    SkeletonModule,
    NgForOf,
    QuestionDetailsComponent,
    DatePipe,
    Button
  ],
  templateUrl: './question-list.component.html',
  styleUrl: './question-list.component.css'
})
export class QuestionListComponent implements OnInit {
  questions: Question[] = [];
  selectedQuestion: Question | null = null;
  displayDialog: boolean = false;
  loading: boolean = true;

  constructor(
    private questionsService: QuestionService,
    private messageService: MessageService
  ) {}

  ngOnInit() {
    this.loadQuestions();
  }

  loadQuestions() {
    this.loading = true;
    this.questionsService.getQuestions().subscribe({
      next: (data) => {
        this.questions = data;
        this.loading = false;
      },
      error: (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Błąd',
          detail: 'Nie udało się załadować pytań'
        });
        this.loading = false;
      }
    });
  }

  showDetails(question: Question) {
    this.selectedQuestion = question;
    this.displayDialog = true;
  }
}
