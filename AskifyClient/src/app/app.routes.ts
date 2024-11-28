import {Routes} from '@angular/router';
import {authGuard} from './guards/auth.guard';

export const routes: Routes = [
  {
    path: 'sign-up',
    loadComponent: () => import('./components/sign-up/sign-up.component').then(m => m.SignUpComponent)
  },
  {
    path: 'sign-in',
    loadComponent: () => import('./components/sign-in/sign-in.component').then(m => m.SignInComponent)
  },
  {
    path: 'home',
    loadComponent: () => import('./components/home/home.component').then(m => m.HomeComponent),
    canActivate: [authGuard]
  }
];
