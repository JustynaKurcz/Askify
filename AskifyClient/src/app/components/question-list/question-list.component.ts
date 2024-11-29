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
import {DatePipe, NgIf} from '@angular/common';
import {QuestionDetailsComponent} from '../question-details/question-details.component';
import {AuthService} from '../../services/auth.service';
import {DialogService, DynamicDialogRef} from 'primeng/dynamicdialog';
import {CreateQuestionComponent} from '../create-question/create-question.component';

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
    QuestionDetailsComponent,
    DatePipe,
    Button,
    NgIf
  ],
  providers: [DialogService],
  templateUrl: './question-list.component.html',
  styleUrl: './question-list.component.css'
})
export class QuestionListComponent implements OnInit {
  ref: DynamicDialogRef | undefined;
  questions: Question[] = [];
  selectedQuestion: Question | null = null;
  displayDialog: boolean = false;
  loading: boolean = true;
  isLoggedIn: boolean = false;

  constructor(
    private questionsService: QuestionService,
    private messageService: MessageService,
    private authService: AuthService,
    private dialogService: DialogService
  ) {}

  ngOnInit() {
    this.loadQuestions();
    this.isLoggedIn = this.authService.isLoggedIn();
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

  addQuestion() {
    this.ref = this.dialogService.open(CreateQuestionComponent, {
      header: 'Dodaj nowe pytanie',
      width: '50vw',
      contentStyle: { overflow: 'auto' },
      breakpoints: {
        '960px': '75vw',
        '640px': '90vw'
      },
      style: {
        'max-height': '90vh'
      }
    });

    this.ref.onClose.subscribe((question) => {
      if (question) {
        console.log('Nowe pytanie:', question);
      }
    });
  }
}
