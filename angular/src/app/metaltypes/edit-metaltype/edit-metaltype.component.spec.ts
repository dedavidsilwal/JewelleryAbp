import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditMetaltypeComponent } from './edit-metaltype.component';

describe('EditMetaltypeComponent', () => {
  let component: EditMetaltypeComponent;
  let fixture: ComponentFixture<EditMetaltypeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditMetaltypeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditMetaltypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
