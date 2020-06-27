import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MetaltypesComponent } from './metaltypes.component';

describe('MetaltypesComponent', () => {
  let component: MetaltypesComponent;
  let fixture: ComponentFixture<MetaltypesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MetaltypesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MetaltypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
