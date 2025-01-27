import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {DialogModule} from 'primeng/dialog';
import {TableModule} from 'primeng/table';
import {MessageService} from 'primeng/api';
import {Question, QuestionService} from '../../services/question.service';
import {MenuModule} from 'primeng/menu';
import {CardModule} from 'primeng/card';
import {InputTextModule} from 'primeng/inputtext';
import {DividerModule} from 'primeng/divider';
import {TagModule} from 'primeng/tag';
import {SkeletonModule} from 'primeng/skeleton';
import {DatePipe, NgForOf, NgIf, NgTemplateOutlet} from '@angular/common';
import {QuestionDetailsComponent} from '../question-details/question-details.component';
import {AuthService} from '../../services/auth.service';
import {DialogService, DynamicDialogRef} from 'primeng/dynamicdialog';
import {CreateQuestionComponent} from '../create-question/create-question.component';
import {debounceTime, distinctUntilChanged, Subject, Subscription} from 'rxjs';
import {PaginatorModule} from 'primeng/paginator';
import {ProgressSpinnerModule} from 'primeng/progressspinner';

interface PaginationParams {
  pageNumber: number;
  pageSize: number;
  search?: string;
}

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
    NgIf,
    NgForOf,
    PaginatorModule,
    ProgressSpinnerModule,
    NgTemplateOutlet
  ],
  providers: [DialogService],
  templateUrl: './question-list.component.html',
  styleUrl: './question-list.component.css'
})
export class QuestionListComponent implements OnInit, OnDestroy {
  private readonly questionsService = inject(QuestionService)
  private readonly messageService = inject(MessageService)
  private readonly authService = inject(AuthService)
  private readonly dialogService = inject(DialogService);
  private readonly searchSubject = new Subject<string>();
  private searchSubscription?: Subscription;
  private readonly defaultPageSize = 10;

  ref?: DynamicDialogRef;
  questionId: string = '';
  displayDialog = false;
  loading = true;
  isLoggedIn = false;
  questions: Question[] = [];
  totalRecords = 0;
  currentPage = 1;
  pageSize = this.defaultPageSize;
  pageSizeOptions = [5, 10, 15];
  searchValue = '';
  isAdmin = false;

  async ngOnInit(): Promise<void> {
    this.isLoggedIn = this.authService.isLoggedIn();
    this.isAdmin = this.authService.isAdmin();

    this.initializeSearchSubscription()
    await this.loadQuestions();
  }

  ngOnDestroy(): void {
    this.searchSubscription?.unsubscribe();
    this.ref?.close();
  }

  private initializeSearchSubscription(): void {
    this.searchSubscription = this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(async (value) => {
      this.searchValue = value;
      this.currentPage = 1;
      await this.loadQuestions();
    });
  }

  private async loadQuestions(): Promise<void> {
    try {
      this.loading = true;
      const params: PaginationParams = {
        pageNumber: this.currentPage,
        pageSize: this.pageSize,
        ...(this.searchValue && {search: this.searchValue})
      };

      const response = await this.questionsService.getQuestions(params).toPromise();
      if (response) {
        this.questions = response.items;
        this.totalRecords = response.totalItems;
      }
    } catch (error) {
      this.handleError(error);
    } finally {
      this.loading = false;
    }
  }

  private handleError(error: unknown): void {
    console.error('Error loading questions:', error);
    this.messageService.add({
      severity: 'error',
      summary: 'Błąd',
      detail: 'Nie udało się załadować pytań'
    });
  }

  async onPageChange(event: any): Promise<void> {
    this.currentPage = event.page + 1;
    this.pageSize = event.rows;
    await this.loadQuestions();
  }

  onSearch(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.searchSubject.next(target.value);
  }

  showDetails(questionId: string): void {
    this.questionId = questionId;
    this.displayDialog = true;
  }

  addQuestion(): void {
    this.ref = this.dialogService.open(CreateQuestionComponent, {
      header: 'Dodaj nowe pytanie',
      width: '600px',
      contentStyle: {overflow: 'auto'},
      styleClass: 'question-dialog'
    });

    this.ref.onClose.subscribe((question: Question) => {
      if (question) {
        void this.loadQuestions();
      }
    });
  }

  deleteQuestion(questionId: string) {
    this.questionsService.deleteQuestion(questionId).subscribe(() => {
      this.messageService.add({
        severity: 'success',
        summary: 'Sukces',
        detail: 'Pytanie zostało usunięte'
      });
      this.loadQuestions();
    }, (error) => {
      console.error('Error deleting question:', error);
      this.messageService.add({
        severity: 'error',
        summary: 'Błąd',
        detail: 'Nie udało się usunąć pytania'
      });
    });
  }
}
