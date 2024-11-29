import {BehaviorSubject, map, Observable, of, switchMap, tap} from 'rxjs';
import {API_CONSTANTS} from '../constants/api';
import {SignUp} from './types/signUp';
import {SignInResponse} from './types/signInResponse';
import {SignIn} from './types/signIn';
import {HttpClient} from '@angular/common/http';
import {inject, Injectable} from '@angular/core';
import {DOCUMENT} from '@angular/common';
import {JwtHelperService} from '@auth0/angular-jwt';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private document = inject(DOCUMENT);
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);

  private localStorage = this.document.defaultView?.localStorage;

  signIn(signInData: SignIn): Observable<any> {
    return this.http
      .post<SignInResponse>(API_CONSTANTS.USERS.SIGN_IN, signInData)
      .pipe(
        switchMap((result: SignInResponse) => {
          if (result?.token) {
            this.localStorage?.setItem('token', result.token);
            this.isAuthenticatedSubject.next(true);
            return this.currentLoggedUser().pipe(
              map(() => true)
            );
          }
          return of(false);
        })
      );
  }

  signUp(signUpData: SignUp): Observable<any> {
    return this.http.post(API_CONSTANTS.USERS.SIGN_UP, signUpData);
  }

  currentLoggedUser(): Observable<any> {
    return this.http.get<any>(API_CONSTANTS.USERS.CURRENT_USER)
      .pipe(
        tap((result: any) => {
          localStorage.setItem('userId', result.id);
        })
      );
  }

  async signOut() {
    this.localStorage?.removeItem('token');
    this.isAuthenticatedSubject.next(false);
  }

  isLoggedIn() {
    const localStorage = this.document.defaultView?.localStorage;
    const jwtHelper = new JwtHelperService();
    const token = localStorage?.getItem('token');
    if (!token) {
      this.isAuthenticatedSubject.next(false);
      localStorage?.removeItem('token');
      return false;
    }
    const isExpired = !jwtHelper.isTokenExpired(token);
    this.isAuthenticatedSubject.next(isExpired);
    return isExpired;
  }

  getAuthState(): Observable<boolean> {
    return this.isAuthenticatedSubject.asObservable();
  }

  getUserName(userId: string): Observable<any> {
    return this.http.get<any>(API_CONSTANTS.USERS.BASE_PATH + `/${userId}/name`);
  }
}
