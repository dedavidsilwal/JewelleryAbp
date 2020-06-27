import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateMetaltypeComponent } from './create-metaltype.component';

describe('CreateMetaltypeComponent', () => {
  let component: CreateMetaltypeComponent;
  let fixture: ComponentFixture<CreateMetaltypeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateMetaltypeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateMetaltypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
