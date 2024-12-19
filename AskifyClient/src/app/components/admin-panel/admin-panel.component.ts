import {Component, OnInit} from '@angular/core';
import {CardModule} from 'primeng/card';
import {TableModule} from 'primeng/table';
import {TagModule} from 'primeng/tag';
import {ToastModule} from 'primeng/toast';
import {ConfirmDialogModule} from 'primeng/confirmdialog';
import {AuthService, PaginationUser, User} from '../../services/auth.service';
import {ConfirmationService, MessageService} from 'primeng/api';
import {DatePipe} from '@angular/common';

@Component({
  selector: 'app-admin-panel',
  standalone: true,
  imports: [
    CardModule,
    TableModule,
    TagModule,
    ToastModule,
    ConfirmDialogModule,
    DatePipe
  ],
  providers: [AuthService, ConfirmationService],
  templateUrl: './admin-panel.component.html',
  styleUrl: './admin-panel.component.css'
})
export class AdminPanelComponent  implements OnInit {
  users: User[] = [];
  loading = false;
  totalRecords = 0;
  pageSize = 10;

  constructor(
    private authService: AuthService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit() {
    this.loadUsers({ first: 0, rows: this.pageSize });
  }

  loadUsers(event: any) {
    this.loading = true;
    const pageNumber = event.first / event.rows;

    this.authService.getUsers({
      pageNumber: pageNumber,
      pageSize: event.rows
    }).subscribe({
      next: (response: PaginationUser) => {
        this.users = response.items;
        this.totalRecords = response.totalItems;
        this.loading = false;
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Błąd',
          detail: 'Nie udało się załadować listy użytkowników'
        });
        this.loading = false;
      }
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
        this.loadUsers({ first: 0, rows: this.pageSize });
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
