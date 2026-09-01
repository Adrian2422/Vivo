import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./views/main-screen/main-screen').then((c) => c.MainScreen),
  },
  {
    path: '**',
    loadComponent: () => import('./views/not-found/not-found').then((c) => c.NotFound),
  },
];
