import { Injectable, OnDestroy, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MinuteTickerService implements OnDestroy {
  private intervalId: ReturnType<typeof setInterval> | null = null;
  
  private readonly _tick = signal<number>(Date.now());
  
  public readonly tick = this._tick.asReadonly();

  constructor() {
    this.startTicker();
  }

  private startTicker(): void {
    this.intervalId = setInterval(() => {
      this._tick.set(Date.now());
      console.log('changed!');
    }, 60000);
  }

  ngOnDestroy(): void {
    if (this.intervalId !== null) {
      clearInterval(this.intervalId);
      this.intervalId = null;
    }
  }
}
