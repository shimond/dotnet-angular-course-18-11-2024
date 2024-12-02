import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CustomerManageComponent } from "./components/customer-manage/customer-manage.component";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CustomerManageComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'use-signals';
}
