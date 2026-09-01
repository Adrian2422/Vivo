import { Component, signal } from '@angular/core';
import { environment } from '../../../environments/environment';

@Component({
  imports: [],
  selector: 'app-header',
  styleUrl: './header.scss',
  templateUrl: './header.html',
})
export class Header {
  protected readonly title = signal(environment.appName);
}
