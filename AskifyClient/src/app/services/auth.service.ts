import {BehaviorSubject, map, Observable, of, switchMap, tap} from 'rxjs';
import {API_CONSTANTS} from '../constants/api';
import {SignUp} from './types/signUp';
import {SignInResponse} from './types/signInResponse';
import {SignIn} from './types/signIn';
import {HttpClient, HttpParams} from '@angular/common/http';
import {inject, Injectable} from '@angular/core';
import {DOCUMENT} from '@angular/common';
import {JwtHelperService} from '@auth0/angular-jwt';

export interface User {
  id: string;
  email: string;
  userName: string;
  createdAt: Date;
  role: string;
}

export type PaginationUser = {
  items: User[];
  totalItems: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

interface QueryParams {
  pageNumber?: number;
  pageSize?: number;
  search?: string;
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
    this.localStorage?.removeItem('userId');
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

  deleteAccount(): Observable<any> {
    return this.http.delete(API_CONSTANTS.USERS.BASE_PATH);
  }

  isAdmin(): boolean {
    const token = this.localStorage?.getItem('token');
    if (!token) return false;

    const jwtHelper = new JwtHelperService();
    try {
      const decodedToken = jwtHelper.decodeToken(token);
      return decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] === 'Admin';
    } catch {
      return false;
    }
  }

  getUsers(params: QueryParams = {}): Observable<PaginationUser> {
    let httpParams = new HttpParams();

    if (params.pageNumber) {
      httpParams = httpParams.set('pageNumber', params.pageNumber.toString());
    }
    if (params.pageSize) {
      httpParams = httpParams.set('pageSize', params.pageSize.toString());
    }
    if (params.search) {
      httpParams = httpParams.set('search', params.search);
    }

    return this.http.get<PaginationUser>(
      API_CONSTANTS.USERS.BASE_PATH,
      {params: httpParams}
    );
  }

  deleteUserById(userId: string): Observable<any> {
    return this.http.delete(`${API_CONSTANTS.USERS.BASE_PATH}/${userId}`);
  }

}
