import { Component, inject } from '@angular/core';
import { patientListStore } from './patient-list.store';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-patient-list',
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule],
  templateUrl: './patient-list.component.html',
  styleUrl: './patient-list.component.scss'
})
export class PatientListComponent {
    displayedColumns = ['id', 'name','select'];
    store = inject(patientListStore);
}
