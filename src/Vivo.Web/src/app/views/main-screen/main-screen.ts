import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Table } from '../../shared/components/table/table';
import { Card } from '../../shared/components/card/card';
import { BreakpointObserver } from '@angular/cdk/layout';
import { firstValueFrom, map } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';
import {
  getRecentShortenedUrlsResource,
  ShortenedUrlsService,
} from '@api/shortened-urls/shortened-urls.service';
import { CreateShortenedUrlRequest } from '@api/model';
import { form, FormField, FormRoot, required } from '@angular/forms/signals';

@Component({
  imports: [FormsModule, Table, Card, FormField, FormRoot],
  selector: 'app-main-screen',
  styleUrl: './main-screen.scss',
  templateUrl: './main-screen.html',
})
export class MainScreen {
  private readonly _breakpointObserver = inject(BreakpointObserver);
  private readonly _apiService = inject(ShortenedUrlsService);

  protected readonly isMobile = toSignal(
    this._breakpointObserver.observe('(max-width: 767px)').pipe(map((result) => result.matches)),
    { initialValue: false },
  );

  protected readonly recentUrls = getRecentShortenedUrlsResource();
  private readonly initialFormState: CreateShortenedUrlRequest = {
    originalUrl: '',
  };
  private readonly _formModel = signal<CreateShortenedUrlRequest>({ ...this.initialFormState });

  protected readonly shortenUrlForm = form(
    this._formModel,
    (schemaPath) => {
      required(schemaPath.originalUrl);
    },
    {
      submission: {
        action: async (field) => {
          await firstValueFrom(this._apiService.createAsync(field().value()));

          this.resetForm();
          this.recentUrls.reload();
        },
        onInvalid: (field) => {
          const firstError = field().errorSummary()[0];
          firstError?.fieldTree().focusBoundControl();
        },
        ignoreValidators: 'none',
      },
    },
  );

  private resetForm(): void {
    this._formModel.set({ ...this.initialFormState });
  }
}
