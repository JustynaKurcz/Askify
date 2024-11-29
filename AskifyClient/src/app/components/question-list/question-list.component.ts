import {Component, OnDestroy, OnInit} from '@angular/core';
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
import {debounceTime, distinctUntilChanged, Subject, Subscription} from 'rxjs';

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
export class QuestionListComponent implements OnInit, OnDestroy {
  private searchSubject = new Subject<string>();
  private searchSubscription: Subscription;

  ref: DynamicDialogRef | undefined;
  selectedQuestion: Question | null = null;
  displayDialog: boolean = false;
  loading: boolean = true;
  isLoggedIn: boolean = false;
  questions: Question[] = [];
  totalRecords: number = 0;
  currentPage: number = 1;
  pageSize: number = 10;
  pageSizeOptions: number[] = [5, 10, 15];
  searchValue: string = '';

  constructor(
    private questionsService: QuestionService,
    private messageService: MessageService,
    private authService: AuthService,
    private dialogService: DialogService
  ) {
    this.searchSubscription = this.searchSubject.pipe(
      debounceTime(500),
      distinctUntilChanged()
    ).subscribe(value => {
      this.searchValue = value;
      this.currentPage = 1;
      this.loadQuestions();
    });
  }

  ngOnInit() {
    this.loadQuestions();
    this.isLoggedIn = this.authService.isLoggedIn();
  }

  ngOnDestroy() {
    if (this.searchSubscription) {
      this.searchSubscription.unsubscribe();
    }
  }

  loadQuestions() {
    this.loading = true;
    const params = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize,
      search: this.searchValue || undefined
    };

    this.questionsService.getQuestions(params).subscribe({
      next: (response) => {
        this.questions = response.items;
        this.totalRecords = response.totalItems;
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

  onPageChange(event: any) {
    this.currentPage = Math.floor(event.first / event.rows) + 1;
    this.pageSize = event.rows;
    this.loadQuestions();
  }

  onSearch(event: any) {
    this.searchSubject.next(event.target.value);
  }

  showDetails(question: Question) {
    this.selectedQuestion = question;
    this.displayDialog = true;
  }

  addQuestion() {
    this.ref = this.dialogService.open(CreateQuestionComponent, {
      header: 'Dodaj nowe pytanie',
      width: '600px',
      contentStyle: { overflow: 'auto' },
    });

    this.ref.onClose.subscribe((question) => {
      if (question) {
        console.log('Nowe pytanie:', question);
      }
    });
  }
}
