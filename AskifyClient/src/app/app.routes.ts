import {Routes} from '@angular/router';
import {authGuard} from './guards/auth.guard';

export const routes: Routes = [
  {
    path: 'zaloguj-sie',
    loadComponent: () => import('./components/sign-up/sign-up.component').then(m => m.SignUpComponent)
  },
  {
    path: 'zarejestruj-sie',
    loadComponent: () => import('./components/sign-in/sign-in.component').then(m => m.SignInComponent)
  },
  {
    path: 'strona-glowna',
    loadComponent: () => import('./components/home/home.component').then(m => m.HomeComponent),
  },
  {
    path: '',
    redirectTo: 'strona-glowna',
    pathMatch: 'full'
  },
  {
    path: 'pytania',
    loadComponent: () => import('./components/question-list/question-list.component').then(m => m.QuestionListComponent),
    canActivate: [authGuard]
  },
  {
    path: 'moje-konto',
    loadComponent: () => import('./components/my-account/my-account.component').then(m => m.MyAccountComponent),
    canActivate: [authGuard]
  },
  {
    path: 'admin-panel',
    loadComponent: () => import('./components/admin-panel/admin-panel.component').then(m => m.AdminPanelComponent),
    // todo: add admin guard
  },
  {
    path: '**',
    loadComponent: () => import('./components/not-found/not-found.component').then(m => m.NotFoundComponent)
  }
];
