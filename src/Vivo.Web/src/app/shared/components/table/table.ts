import { Component, inject, input } from '@angular/core';
import { LucideCopy } from '@lucide/angular';
import { Clipboard } from '@angular/cdk/clipboard';
import { ShortenedUrlResponse } from '@api/model';

@Component({
  imports: [LucideCopy],
  selector: 'app-table',
  styleUrl: './table.scss',
  templateUrl: './table.html',
})
export class Table {
  private readonly _clipboard = inject(Clipboard);

  public readonly data = input.required<ShortenedUrlResponse[]>();

  protected copyToClipboard(shortUrl: string): void {
    this._clipboard.copy(shortUrl);
  }
}
