import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SaleDueDetailComponent } from './sale-due-detail.component';

describe('SaleDueDetailComponent', () => {
  let component: SaleDueDetailComponent;
  let fixture: ComponentFixture<SaleDueDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SaleDueDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SaleDueDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
