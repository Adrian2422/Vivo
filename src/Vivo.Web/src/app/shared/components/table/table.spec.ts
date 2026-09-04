import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Table } from './table';
import { ComponentRef } from '@angular/core';

describe('Table', () => {
  let component: Table;
  let fixture: ComponentFixture<Table>;
  let componentRef: ComponentRef<Table>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Table],
    }).compileComponents();

    fixture = TestBed.createComponent(Table);
    component = fixture.componentInstance;
    componentRef = fixture.componentRef;

    componentRef.setInput('data', []);
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
