import { Component, inject } from '@angular/core';
import { LucideCopy } from '@lucide/angular';
import { Clipboard } from '@angular/cdk/clipboard';

@Component({
  imports: [LucideCopy],
  selector: 'app-table',
  styleUrl: './table.scss',
  templateUrl: './table.html',
})
export class Table {
  private readonly _clipboard = inject(Clipboard);

  protected copyToClipboard(text: string): void {
    this._clipboard.copy(text);
  }
}
