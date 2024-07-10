import { NgForOf } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatListModule } from '@angular/material/list';

@Component({
  selector: 'app-error-list',
  standalone: true,
  imports: [ NgForOf, MatDialogModule, MatListModule ],
  templateUrl: './error-list.component.html',
  styleUrl: './error-list.component.scss'
})
export class ErrorListComponent {
  constructor (
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {  }
}
