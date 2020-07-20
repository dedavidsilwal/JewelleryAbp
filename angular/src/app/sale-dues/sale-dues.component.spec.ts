import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SaleDuesComponent } from './sale-dues.component';

describe('SaleDuesComponent', () => {
  let component: SaleDuesComponent;
  let fixture: ComponentFixture<SaleDuesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SaleDuesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SaleDuesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
