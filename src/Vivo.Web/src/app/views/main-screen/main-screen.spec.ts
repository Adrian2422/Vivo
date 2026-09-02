import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MainScreen } from './main-screen';
import { provideHttpClient } from '@angular/common/http';
import { setupServer } from 'msw/node';
import { getShortenedUrlsMock } from '@api/shortened-urls/shortened-urls.msw';
import { BehaviorSubject } from 'rxjs';
import { BreakpointObserver, BreakpointState } from '@angular/cdk/layout';

const server = setupServer(...getShortenedUrlsMock());

describe('MainScreen', () => {
  let fixture: ComponentFixture<MainScreen>;
  let breakpointSubject: BehaviorSubject<BreakpointState>;

  beforeAll(() => server.listen({ onUnhandledRequest: 'error' }));
  afterEach(() => server.resetHandlers());
  afterAll(() => server.close());

  beforeEach(async () => {
    breakpointSubject = new BehaviorSubject<BreakpointState>({
      matches: false,
      breakpoints: {},
    });

    await TestBed.configureTestingModule({
      imports: [MainScreen],
      providers: [
        provideHttpClient(),
        {
          provide: BreakpointObserver,
          useValue: {
            observe: () => breakpointSubject.asObservable(),
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(MainScreen);
  });

  it('powinien wyświetlić tabelę w trybie desktop', async () => {
    breakpointSubject.next({ matches: false, breakpoints: {} });

    fixture.detectChanges();
    await fixture.whenStable();

    const tableElement = fixture.nativeElement.querySelector('app-table');
    const cardElement = fixture.nativeElement.querySelector('app-card');

    expect(tableElement).toBeTruthy();
    expect(cardElement).toBeNull();
  });

  it('powinien wyświetlić karty w trybie mobile', async () => {
    breakpointSubject.next({ matches: true, breakpoints: {} });

    fixture.detectChanges();
    await fixture.whenStable();

    const tableElement = fixture.nativeElement.querySelector('app-table');
    const cardElement = fixture.nativeElement.querySelector('app-card');

    expect(cardElement).toBeTruthy();
    expect(tableElement).toBeNull();
  });
});
