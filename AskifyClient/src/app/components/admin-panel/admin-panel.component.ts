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

interface PaginationParams {
  pageNumber: number;
  pageSize: number;
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
    DatePipe
  ],
  providers: [AuthService, ConfirmationService, MessageService],
  templateUrl: './admin-panel.component.html',
  styleUrl: './admin-panel.component.css'
})
export class AdminPanelComponent implements OnInit {
  users: User[] = [];
  loading = false;
  totalRecords = 0;
  first = 0;
  pageSize = 10;
  pageSizeOptions = [5, 10, 15, 20];

  constructor(
    private authService: AuthService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit() {
    this.loadUsers();
  }

  onPageChange(event: any) {
    this.first = event.first;
    this.pageSize = event.rows;
    this.loadUsers();
  }

  async loadUsers() {
    try {
      this.loading = true;
      const pageNumber = Math.floor(this.first / this.pageSize) + 1;

      const response = await this.authService.getUsers({
        pageNumber: pageNumber,
        pageSize: this.pageSize
      }).toPromise();

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
