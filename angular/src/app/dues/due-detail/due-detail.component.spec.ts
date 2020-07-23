import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DueDetailComponent } from './due-detail.component';

describe('DueDetailComponent', () => {
  let component: DueDetailComponent;
  let fixture: ComponentFixture<DueDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DueDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DueDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
