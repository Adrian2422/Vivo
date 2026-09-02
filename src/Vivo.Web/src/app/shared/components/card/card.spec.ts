import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Card } from './card';
import { ComponentRef } from '@angular/core';
import { ShortenedUrlResponse } from '@api/model';
import { environment } from '../../../../environments/environment';

describe('Card', () => {
  let component: Card;
  let fixture: ComponentFixture<Card>;
  let componentRef: ComponentRef<Card>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Card],
    }).compileComponents();

    fixture = TestBed.createComponent(Card);
    component = fixture.componentInstance;
    componentRef = fixture.componentRef;

    componentRef.setInput('shortenedUrl', {
      id: crypto.randomUUID(),
      code: 'abcd123',
      originalUrl: 'https://wp.pl',
      shortUrl: `${environment.apiUrl}abcd123`,
      clickCount: 0,
      createdAt: new Date().toUTCString(),
    } as ShortenedUrlResponse);

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
