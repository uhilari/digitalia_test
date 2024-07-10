import { Component, Inject, Input } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-alert',
  standalone: true,
  imports: [ MatDialogModule ],
  templateUrl: './alert.component.html',
  styleUrl: './alert.component.scss'
})
export class AlertComponent {
  constructor (
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {  }
}
