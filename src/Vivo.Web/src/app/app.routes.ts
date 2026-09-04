import { Routes } from '@angular/router';
import { ShortenedUrlsService } from '@api/shortened-urls/shortened-urls.service';

export const routes: Routes = [
  {
    path: '',
    providers: [ShortenedUrlsService],
    loadComponent: () => import('./views/main-screen/main-screen').then((c) => c.MainScreen),
  },
  {
    path: '**',
    loadComponent: () => import('./views/not-found/not-found').then((c) => c.NotFound),
  },
];
