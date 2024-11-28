import {Component, inject, OnInit} from '@angular/core';
import {Button} from 'primeng/button';
import {CardModule} from 'primeng/card';
import {InputTextModule} from 'primeng/inputtext';
import {NgClass, NgIf} from '@angular/common';
import {PaginatorModule} from 'primeng/paginator';
import {PasswordModule} from 'primeng/password';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {AuthService} from '../../services/auth.service';
import {ToastService} from '../../services/toast.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [
    Button,
    CardModule,
    InputTextModule,
    NgIf,
    PaginatorModule,
    PasswordModule,
    ReactiveFormsModule,
    NgClass
  ],
  providers: [AuthService, ToastService],
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.css'
})
export class SignUpComponent implements OnInit{
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly toastService = inject(ToastService);
  private readonly router = inject(Router);

  signUpForm!: FormGroup;

  ngOnInit() : void {
    this.initializeForm();
  }

  initializeForm() : void {
    this.signUpForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      userName: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }


  onSubmit() {
    if (this.signUpForm.valid) {
      this.authService.signUp(this.signUpForm.value)
        .subscribe({
          next: () => this.router.navigate(['/sign-in']),
          error: () => {
            this.toastService.showWarning('Wystąpił błąd podczas rejestracji');
          }
        });
    }
  }
}
