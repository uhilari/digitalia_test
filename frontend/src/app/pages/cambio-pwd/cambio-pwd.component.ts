import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { PerfilService } from '../../services/perfil.service';
import { AlertComponent } from '../../components/alert/alert.component';
import { ErrorListComponent } from '../../components/error-list/error-list.component';
import { MustMatch } from '../../services/helpers';

@Component({
  selector: 'app-cambio-pwd',
  standalone: true,
  imports: [MatCardModule, MatDialogModule, MatInputModule, MatButtonModule, ReactiveFormsModule],
  templateUrl: './cambio-pwd.component.html',
  styleUrl: './cambio-pwd.component.scss'
})
export class CambioPwdComponent {
  constructor (
    private dialog: MatDialog,
    private perfil: PerfilService
  ) { }

  grupo = new FormGroup({
    antiguoPassword: new FormControl('', [Validators.required]),
    nuevoPassword: new FormControl('', [Validators.required]),
    confirmaPassword: new FormControl('', [Validators.required])
  }, {
    validators: MustMatch('nuevoPassword', 'confirmaPassword')
  });

  onSubmitCambioPwd() {
    if (this.grupo.valid) {
      this.perfil.cambioPwd(this.grupo.value)
        .subscribe({
          next: _ => {
            this.grupo.reset();
            this.dialog.open(AlertComponent, {
              data: {
                titulo: 'Cambiar Contraseña',
                mensaje: 'Se ha cambiado la contraseña'
              }
            });
          },
          error: e => {
            this.grupo.reset();
            this.dialog.open(ErrorListComponent, {
              data: {
                titulo: 'Error al Cambiar contraseña',
                errores: e.error
              }
            });
          }
        })
    }
  }
}
