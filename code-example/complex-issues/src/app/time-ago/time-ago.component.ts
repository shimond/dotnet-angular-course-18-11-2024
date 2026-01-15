import { Component, computed, inject, input } from '@angular/core';
import { MinuteTickerService } from '../minute-ticker.service';

@Component({
  selector: 'app-time-ago',
  standalone: true,
  imports: [],
  templateUrl: './time-ago.component.html',
  styleUrl: './time-ago.component.scss'
})
export class TimeAgoComponent {
  private readonly tickerService = inject(MinuteTickerService);

  public readonly timestamp = input.required<Date | number | string>();

  public readonly timeAgo = computed(() => {
    this.tickerService.tick();
    const inputValue = this.timestamp();
    const targetDate = this.normalizeDate(inputValue);
    const now = new Date();
    return this.calculateTimeAgo(targetDate, now);
  });

  private normalizeDate(value: Date | number | string): Date {
    if (value instanceof Date) {
      return value;
    }
    if (typeof value === 'number') {
      return new Date(value);
    }
    return new Date(value);
  }

  private calculateTimeAgo(targetDate: Date, now: Date): string {
    const diffMs = now.getTime() - targetDate.getTime();
    const diffSeconds = Math.floor(diffMs / 1000);

    if (diffSeconds < 60) {
      return 'just now';
    }

    const diffMinutes = Math.floor(diffSeconds / 60);
    if (diffMinutes < 60) {
      return `${diffMinutes} ${this.pluralize('minute', diffMinutes)} ago`;
    }

    const diffHours = Math.floor(diffMinutes / 60);
    if (diffHours < 24) {
      return `${diffHours} ${this.pluralize('hour', diffHours)} ago`;
    }

    const diffDays = Math.floor(diffHours / 24);
    if (diffDays < 7) {
      return `${diffDays} ${this.pluralize('day', diffDays)} ago`;
    }

    const diffWeeks = Math.floor(diffDays / 7);
    if (diffWeeks < 4) {
      return `${diffWeeks} ${this.pluralize('week', diffWeeks)} ago`;
    }

    const diffMonths = Math.floor(diffDays / 30);
    if (diffMonths < 12) {
      return `${diffMonths} ${this.pluralize('month', diffMonths)} ago`;
    }

    const diffYears = Math.floor(diffDays / 365);
    return `${diffYears} ${this.pluralize('year', diffYears)} ago`;
  }

  private pluralize(word: string, count: number): string {
    return count === 1 ? word : `${word}s`;
  }
}
