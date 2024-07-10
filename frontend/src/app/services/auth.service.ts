import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { map } from 'rxjs';
import { TokenResponse } from '../models/token-response';

const IsAuthKey = 'app-is-auth';
const TokenKey = 'app-token';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private http: HttpClient
  ) {
    this.isAuth = (sessionStorage.getItem(IsAuthKey)?.toLowerCase() === 'true');
    this.token = sessionStorage.getItem(TokenKey) ?? '';
  }

  public isAuth: boolean = false;
  public token: string = '';

  login(username: string, password: string) {
    const url = `${environment.urlApi}/token`;
    return this.http.post<TokenResponse>(url, { username, password })
      .pipe(map(r => {
        this.isAuth = true;
        this.token = r.token;
        sessionStorage.setItem(IsAuthKey, 'true');
        sessionStorage.setItem(TokenKey, r.token);
        return {};
      }));
  }

  logout() {
    this.isAuth = false;
    this.token = '';
    sessionStorage.removeItem(IsAuthKey);
    sessionStorage.removeItem(TokenKey);
}

  registro(data: any) {
    const url = `${environment.urlApi}/registro`;
    return this.http.post(url, data);
  }
}
