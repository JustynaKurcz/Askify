import {HttpHeaders, HttpInterceptorFn} from '@angular/common/http';
import {JwtHelperService} from '@auth0/angular-jwt';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('token');
  const jwtHelper = new JwtHelperService();

  if (token && !jwtHelper.isTokenExpired(token)) {
    return next(req.clone({
      headers: new HttpHeaders().set('Authorization', `Bearer ${token}`)
    }));
  }

  localStorage.removeItem('token');
  localStorage.removeItem('userId');
  return next(req);
};
