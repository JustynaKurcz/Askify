import {BehaviorSubject, map, Observable} from 'rxjs';
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
}
