import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { PerfilService } from '../../services/perfil.service';
import { ErrorListComponent } from '../../components/error-list/error-list.component';
import { AlertComponent } from '../../components/alert/alert.component';

@Component({
  selector: 'app-perfil',
  standalone: true,
  imports: [MatCardModule, MatDialogModule, MatInputModule, MatButtonModule, ReactiveFormsModule],
  templateUrl: './perfil.component.html',
  styleUrl: './perfil.component.scss'
})
export class PerfilComponent implements OnInit {
  constructor (
    private dialog: MatDialog,
    private perfil: PerfilService
  ) { }

  grupo = new FormGroup({
    nombres: new FormControl('', [Validators.required]),
    apellidos: new FormControl('', []),
    email: new FormControl('', [Validators.required])
  });

  ngOnInit(): void {
      this.perfil.obtener()
        .subscribe({
          next: r => { this.grupo.setValue(r as any); },
          error: (e) => {
            this.dialog.open(ErrorListComponent, {
              data: {
                titulo: 'Error al obtener los datos',
                errores: e.error
              }
            });
          }
        });
  }

  onSubmitRegistro() {
    if (this.grupo.valid) {
      this.perfil.actualizar(this.grupo.value)
        .subscribe({
          next: _ => {
            this.dialog.open(AlertComponent, {
              data: {
                titulo: 'Grabar Perfil',
                mensaje: 'Se han grabado los datos'
              }
            });
          },
          error: e => {
            this.dialog.open(ErrorListComponent, {
              data: {
                titulo: 'Error al Gabar',
                errores: e.error
              }
            });
          }
        })
    }
  }
}
