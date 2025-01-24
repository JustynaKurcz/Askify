import {Component, inject, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {CardModule} from 'primeng/card';
import {InputTextModule} from 'primeng/inputtext';
import {NgClass, NgIf} from '@angular/common';
import {PasswordModule} from 'primeng/password';
import {Button} from 'primeng/button';
import {AuthService} from '../../services/auth.service';
import {Router, RouterLink} from '@angular/router';
import {ToastService} from '../../services/toast.service';

@Component({
  selector: 'app-sign-in',
  standalone: true,
  templateUrl: './sign-in.component.html',
  imports: [
    CardModule,
    ReactiveFormsModule,
    InputTextModule,
    NgClass,
    NgIf,
    PasswordModule,
    Button,
    RouterLink
  ],
  providers: [AuthService, ToastService],
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent implements OnInit{
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly toastService = inject(ToastService);
  private readonly router = inject(Router);

  signInForm!: FormGroup;

  ngOnInit() : void {
    this.initializeForm();
  }

  initializeForm() : void {
    this.signInForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }


  onSubmit() {
    if (this.signInForm.valid) {
      this.authService.signIn(this.signInForm.value)
        .subscribe({
          next: () => this.router.navigate(['/strona-glowna']),
          error: () => {
           this.toastService.showWarning('Błędne dane logowania');
          }
        });
    }
  }

  onForgotPassword() {

  }
}
