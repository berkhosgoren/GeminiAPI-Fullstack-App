import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PreviousQuestionsComponent } from './previous-questions.component';

describe('PreviousQuestionsComponent', () => {
  let component: PreviousQuestionsComponent;
  let fixture: ComponentFixture<PreviousQuestionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PreviousQuestionsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PreviousQuestionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
