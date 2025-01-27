import { Component, OnInit } from '@angular/core';
import { CardModule } from 'primeng/card';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { PaginatorModule } from 'primeng/paginator';
import { FormsModule } from '@angular/forms';
import { AuthService, User } from '../../services/auth.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DatePipe } from '@angular/common';
import {debounceTime, distinctUntilChanged, Subject, Subscription} from 'rxjs';
import {DynamicDialogRef} from 'primeng/dynamicdialog';
import {InputTextModule} from 'primeng/inputtext';

interface PaginationParams {
  pageNumber: number;
  pageSize: number;
  search?: string;
}

@Component({
  selector: 'app-admin-panel',
  standalone: true,
  imports: [
    CardModule,
    TableModule,
    TagModule,
    ToastModule,
    ConfirmDialogModule,
    PaginatorModule,
    FormsModule,
    DatePipe,
    InputTextModule
  ],
  providers: [AuthService, ConfirmationService, MessageService],
  templateUrl: './admin-panel.component.html',
  styleUrl: './admin-panel.component.css'
})
export class AdminPanelComponent implements OnInit {
  private readonly searchSubject = new Subject<string>();
  private searchSubscription?: Subscription;

  ref?: DynamicDialogRef;
  users: User[] = [];
  loading = false;
  totalRecords = 0;
  currentPage = 1;
  first = 0;
  pageSize = 10;
  pageSizeOptions = [5, 10, 15, 20];
  searchValue = '';

  constructor(
    private authService: AuthService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {}

  async ngOnInit(): Promise<void> {
    this.initializeSearchSubscription()
    await this.loadUsers();
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
      await this.loadUsers();
    });
  }

  async onPageChange(event: any): Promise<void> {
    this.first = event.first;
    this.pageSize = event.rows;
    await this.loadUsers();
  }

  private async loadUsers(): Promise<void> {
    try {
      this.loading = true;
      const params: PaginationParams = {
        pageNumber: this.currentPage,
        pageSize: this.pageSize,
        ...(this.searchValue && {search: this.searchValue})
      };

      const response = await this.authService.getUsers(params).toPromise();
      if (response) {
        this.users = response.items;
        this.totalRecords = response.totalItems;
      }
    } catch (error) {
      this.handleError(error);
    } finally {
      this.loading = false;
    }
  }

  private handleError(error: unknown): void {
    console.error('Error loading users:', error);
    this.messageService.add({
      severity: 'error',
      summary: 'Błąd',
      detail: 'Nie udało się załadować użytkowników'
    });
  }

  onSearch(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.searchSubject.next(target.value);
  }

  getRoleSeverity(role: string): 'success' | 'info' | 'warning' | 'danger' | 'secondary' | 'contrast' {
    switch (role) {
      case 'Admin':
        return 'danger';
      case 'User':
        return 'info';
      default:
        return 'warning';
    }
  }

  confirmDelete(user: User) {
    this.confirmationService.confirm({
      message: `Czy na pewno chcesz usunąć użytkownika ${user.userName}?`,
      header: 'Potwierdzenie usunięcia',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Tak',
      rejectLabel: 'Nie',
      accept: () => this.deleteUser(user.id)
    });
  }

  deleteUser(userId: string) {
    this.authService.deleteUserById(userId).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Sukces',
          detail: 'Użytkownik został usunięty'
        });
        this.loadUsers();
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Błąd',
          detail: 'Nie udało się usunąć użytkownika'
        });
      }
    });
  }
}
