import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Table } from '../../shared/components/table/table';
import { Card } from '../../shared/components/card/card';
import { BreakpointObserver } from '@angular/cdk/layout';
import { map } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';

@Component({
  imports: [FormsModule, Table, Card],
  selector: 'app-main-screen',
  styleUrl: './main-screen.scss',
  templateUrl: './main-screen.html',
})
export class MainScreen {
  private readonly _breakpointObserver = inject(BreakpointObserver);

  protected readonly isMobile = toSignal(
    this._breakpointObserver.observe('(max-width: 767px)').pipe(map((result) => result.matches)),
    { initialValue: false },
  );

  protected send(): void {
    console.log('sent');
  }
}
