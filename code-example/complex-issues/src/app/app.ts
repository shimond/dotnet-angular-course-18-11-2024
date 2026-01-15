import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TimeAgoComponent } from './time-ago/time-ago.component';

@Component({
  selector: 'app-root',
  imports: [TimeAgoComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('complex-issues');
  
  protected readonly now = signal(Date.now());
  protected readonly twoMinutesAgo = signal(Date.now() - 2 * 60 * 1000);
  protected readonly oneHourAgo = signal(Date.now() - 60 * 60 * 1000);
  protected readonly threeDaysAgo = signal(Date.now() - 3 * 24 * 60 * 60 * 1000);
  protected readonly twoWeeksAgo = signal(Date.now() - 14 * 24 * 60 * 60 * 1000);
  protected readonly sixMonthsAgo = signal(Date.now() - 180 * 24 * 60 * 60 * 1000);
  protected readonly oneYearAgo = signal(Date.now() - 365 * 24 * 60 * 60 * 1000);
}
