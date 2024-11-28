import {BehaviorSubject, map, Observable, tap} from 'rxjs';
import {API_CONSTANTS} from '../constants/api';
import {SignUp} from './types/signUp';
import {SignInResponse} from './types/signInResponse';
import {SignIn} from './types/signIn';
import {HttpClient} from '@angular/common/http';
import {inject, Injectable} from '@angular/core';
import {DOCUMENT} from '@angular/common';
import {JwtHelperService} from '@auth0/angular-jwt';


export type User = {
  id: string;
  email: string;
  userName: string;
  createdAt: Date;
  role: string;
}


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
        map((result: SignInResponse) => {
          if (result?.token) {
            this.localStorage?.setItem('token', result.token);
            this.isAuthenticatedSubject.next(true);
            return true;
          }
          return false;
        })
      );
  }

  signUp(signUpData: SignUp): Observable<any> {
    return this.http.post(API_CONSTANTS.USERS.SIGN_UP, signUpData);
  }

  async signOut() {
    this.localStorage?.removeItem('token');
    this.isAuthenticatedSubject.next(false);
  }

  getCurrentUser() {
    return this.http.get<User>(API_CONSTANTS.USERS.BASE_PATH).pipe(
      tap(user => {
        localStorage.setItem('userId', user.id);
        localStorage.setItem('userName', user.userName);
      })
    );
  }

  getCurrentUserId(): string | null {
    return localStorage.getItem('userId');
  }

  getCurrentUserName(): string | null {
    return localStorage.getItem('userName');
  }

  isLoggedIn() {
    const localStorage = this.document.defaultView?.localStorage;
    const jwtHelper = new JwtHelperService();
    const token = localStorage?.getItem('token');
    if (!token) {
      this.isAuthenticatedSubject.next(false);
      return false;
    }
    const isExpired = !jwtHelper.isTokenExpired(token);
    this.isAuthenticatedSubject.next(isExpired);
    return isExpired;
  }

  getAuthState(): Observable<boolean> {
    return this.isAuthenticatedSubject.asObservable();
  }

  getUserName(userId: string) : Observable<any> {
    return this.http.get<any>(API_CONSTANTS.USERS.BASE_PATH + `/${userId}/name`);
  }
}
