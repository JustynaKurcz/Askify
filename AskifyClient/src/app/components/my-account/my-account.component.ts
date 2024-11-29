import {Component, OnInit} from '@angular/core';
import {MessageService} from 'primeng/api';
import {SkeletonModule} from 'primeng/skeleton';
import {NgIf} from '@angular/common';
import {CardModule} from 'primeng/card';
import {TagModule} from 'primeng/tag';
import {AuthService} from '../../services/auth.service';
import {catchError, tap, throwError} from 'rxjs';

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
    TagModule
  ],
  providers: [AuthService],
  templateUrl: './my-account.component.html',
  styleUrl: './my-account.component.css'
})
export class MyAccountComponent implements OnInit {
  userAccount!: UserAccount;
  loading: boolean = true;

  constructor(private messageService: MessageService,  private authService: AuthService) {}

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
}
