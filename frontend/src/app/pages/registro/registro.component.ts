import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { AlertComponent } from '../../components/alert/alert.component';
import { ErrorListComponent } from '../../components/error-list/error-list.component';

@Component({
  selector: 'app-registro',
  standalone: true,
  imports: [ MatCardModule, MatDialogModule, MatInputModule, MatButtonModule, ReactiveFormsModule ],
  templateUrl: './registro.component.html',
  styleUrl: './registro.component.scss'
})
export class RegistroComponent {
  constructor (
    private dialog: MatDialog,
    private auth: AuthService,
    private router: Router
  ) { }

  grupo = new FormGroup({
    username: new FormControl('', [Validators.required]),
    nombres: new FormControl('', [Validators.required]),
    apellidos: new FormControl('', []),
    email: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  });

  onSubmitRegistro() {
    if (this.grupo.valid) {
      this.auth.registro(this.grupo.value)
        .subscribe({
          next: (r) => {
            const dialogRef = this.dialog.open(AlertComponent, {
              data: {
                titulo: 'Registro',
                mensaje: 'Se ha registrado al usuario\nIngrese con sus credenciales'
              }
            });
            dialogRef.afterClosed().subscribe(_ => { this.router.navigate(['/login']); });
          },
          error: (e) => {
            this.grupo.get('password')?.reset();
            this.dialog.open(ErrorListComponent, {
              data: {
                titulo: 'Error al registrar',
                errores: e.error
              }
            });
          },
        });
    }
  }
}
