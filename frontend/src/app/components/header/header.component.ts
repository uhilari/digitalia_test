import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [ MatButtonModule, MatIconModule, MatMenuModule, MatToolbarModule ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  constructor(
    private readonly router: Router,
    private readonly auth: AuthService
  ) { }

  onClickPerfil() {
    this.router.navigate(['/perfil']);
  }

  onClickCambioPwd() {
    this.router.navigate(['/cambio-pwd']);
  }

  onClickSalir() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
