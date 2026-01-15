import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PatientListComponent } from "./pages/patient-list/patient-list.component";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, PatientListComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'front-end-ui-manager';
}
