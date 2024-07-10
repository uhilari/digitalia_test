import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PerfilService {

  constructor(
    private auth: AuthService,
    private http: HttpClient
  ) { }

  private getHeaders() {
    return {
      headers: {
        Authorization: `Bearer ${this.auth.token}`
      }
    };
  }

  obtener() {
    return this.http.get(`${environment.urlApi}/usuario/actual`, this.getHeaders());
  }

  actualizar(data: any) {
    return this.http.patch(`${environment.urlApi}/usuario`, data, this.getHeaders());
  }

  cambioPwd(data: any) {
    return this.http.post(`${environment.urlApi}/usuario/cambio-pwd`, data, this.getHeaders());
  }
}
