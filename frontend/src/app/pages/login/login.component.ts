import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { AlertComponent } from '../../components/alert/alert.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ MatCardModule, MatDialogModule, MatInputModule, MatButtonModule, FormsModule, ReactiveFormsModule, AlertComponent ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  constructor(
    private dialog: MatDialog,
    private auth: AuthService,
    private router: Router
  ) { }

  grupo = new FormGroup({
    username: new FormControl('', [ Validators.required ]),
    password: new FormControl('', [ Validators.required ])
  })

  onClickIngresar() {
    if (this.grupo.valid) {
      this.auth.login(this.grupo.value.username ?? '', this.grupo.value.password ?? '')
        .subscribe({
          next: (r) => {
            this.router.navigate(['/']);
          },
          error: (e) => {
            this.grupo.reset();
            this.dialog.open(AlertComponent, {
              data: {
                titulo: 'Ingresar',
                mensaje: 'Las credenciales de acceso no son válidas'
              }
            });
          }
        });
    }
  }
}
