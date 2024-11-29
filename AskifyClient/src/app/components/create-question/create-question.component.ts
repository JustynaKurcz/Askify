import {Component} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {DynamicDialogRef} from 'primeng/dynamicdialog';
import {InputTextModule} from 'primeng/inputtext';
import {InputTextareaModule} from 'primeng/inputtextarea';
import {Button} from 'primeng/button';
import {NgIf} from '@angular/common';
import {CreateQuestion, QuestionService} from '../../services/question.service';

@Component({
  selector: 'app-create-question',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    InputTextModule,
    InputTextareaModule,
    Button,
    NgIf
  ],
  templateUrl: './create-question.component.html',
  styleUrl: './create-question.component.css'
})
export class CreateQuestionComponent {
  questionForm: FormGroup;
  submitting = false;

  constructor(
    private fb: FormBuilder,
    private ref: DynamicDialogRef,
    private questionService: QuestionService
  ) {
    this.questionForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(250)]],
      content: ['', [Validators.required, Validators.maxLength(5000)]]
    });
  }

  onSubmit() {
    if (this.questionForm.valid) {
      const newQuestion = this.questionForm.value;
      this.questionService.createQuestion(newQuestion as CreateQuestion)
        .subscribe({
          next: () => {
            this.ref.close(newQuestion);
            this.submitting = true;
          }
        });
    }
  }

  cancel() {
    this.ref.close();
  }

  get contentLength(): number {
    return this.questionForm.get('content')?.value?.length || 0;
  }

  get titleLength(): number {
    return this.questionForm.get('title')?.value?.length || 0;
  }
}
