import { Component, input } from '@angular/core';
import { LucideCopy } from '@lucide/angular';
import { ShortenedUrlResponse } from '@api/model';

@Component({
  imports: [LucideCopy],
  selector: 'app-card',
  styleUrl: './card.scss',
  templateUrl: './card.html',
})
export class Card {
  public readonly shortenedUrl = input.required<ShortenedUrlResponse>();
}
