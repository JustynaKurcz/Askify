import {Component, OnInit} from '@angular/core';
import {ConfirmationService, MessageService} from 'primeng/api';
import {SkeletonModule} from 'primeng/skeleton';
import {NgIf} from '@angular/common';
import {CardModule} from 'primeng/card';
import {TagModule} from 'primeng/tag';
import {AuthService} from '../../services/auth.service';
import {catchError, tap, throwError} from 'rxjs';
import {ButtonModule} from 'primeng/button';
import {ConfirmDialogModule} from 'primeng/confirmdialog';
import {ToastModule} from 'primeng/toast';
import {DockModule} from 'primeng/dock';
import {Router} from '@angular/router';

interface UserAccount {
  id: string;
  email: string;
  userName: string;
  createdAt: string;
  role: string;
}

@Component({
  selector: 'app-my-account',
  standalone: true,
  imports: [
    SkeletonModule,
    NgIf,
    CardModule,
    TagModule,
    ButtonModule,
    ConfirmDialogModule,
    ToastModule,
    DockModule
  ],
  providers: [AuthService, ConfirmationService],
  templateUrl: './my-account.component.html',
  styleUrl: './my-account.component.css'
})
export class MyAccountComponent implements OnInit {
  userAccount!: UserAccount;
  loading: boolean = true;

  constructor(private messageService: MessageService,  private authService: AuthService, private confirmationService: ConfirmationService, private router: Router) {}

  ngOnInit() {
    this.loading = true;
    this.authService.currentLoggedUser()
      .pipe(
        tap((userData) => {
          this.userAccount = userData;
          this.loading = false;
        }),
        catchError((error) => {
          this.loading = false;
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Failed to load user data'
          });
          return throwError(() => error);
        })
      )
      .subscribe();
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString();
  }

  confirmDelete() {
    this.confirmationService.confirm({
      message: 'Czy na pewno chcesz usunąć swoje konto? Tej operacji nie można cofnąć.',
      acceptLabel: 'Tak',
      rejectLabel: 'Nie',
      accept: () => {
        this.loading = true;
        this.authService.deleteAccount()
          .pipe(
            tap(() => {
              this.messageService.add({
                severity: 'success',
                summary: 'Sukces',
                detail: 'Konto zostało pomyślnie usunięte'
              });
              this.authService.signOut().then(() => {
                this.router.navigate(['/']);
              });
            }),
            catchError((error) => {
              this.loading = false;
              this.messageService.add({
                severity: 'error',
                summary: 'Błąd',
                detail: 'Nie udało się usunąć konta'
              });
              return throwError(() => error);
            })
          )
          .subscribe();
      }
    });
  }
}
