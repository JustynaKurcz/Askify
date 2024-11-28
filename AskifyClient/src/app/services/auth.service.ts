import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {DOCUMENT} from '@angular/common';
import {map, Observable} from 'rxjs';
import {API_CONSTANTS} from '../constants/api';
import {SignInResponse} from './types/signInResponse';
import {SignIn} from './types/signIn';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private document = inject(DOCUMENT);

  localStorage = this.document.defaultView?.localStorage;

  signIn(signInData: SignIn): Observable<any> {
    return this.http
      .post<SignInResponse>(API_CONSTANTS.USERS.SIGN_IN, signInData)
      .pipe(
        map((result: SignInResponse) => {
          if (result && result.token) {
            this.localStorage?.setItem('token', String(result.token));
            return true;
          }
          return false;
        }),
      );
  }

}
