import {Component, EventEmitter, Input, Output} from '@angular/core';
import {SidebarModule} from 'primeng/sidebar';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {InputTextareaModule} from 'primeng/inputtextarea';
import {NgIf} from '@angular/common';
import {Button} from 'primeng/button';

@Component({
  selector: 'app-create-answer',
  standalone: true,
  imports: [
    SidebarModule,
    ReactiveFormsModule,
    InputTextareaModule,
    NgIf,
    Button
  ],
  templateUrl: './create-answer.component.html',
  styleUrl: './create-answer.component.css'
})
export class CreateAnswerComponent {
  @Input() visible: boolean = false;
  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() answerSubmitted = new EventEmitter<any>();

  answerForm: FormGroup;
  submitting = false;

  constructor(private fb: FormBuilder) {
    this.answerForm = this.fb.group({
      content: ['', [Validators.required, Validators.maxLength(5000)]]
    });
  }

  get contentLength(): number {
    return this.answerForm.get('content')?.value?.length || 0;
  }

  onSubmit() {
    if (this.answerForm.valid) {
      this.submitting = true;
      this.answerSubmitted.emit(this.answerForm.value);
      this.answerForm.reset();
      this.onSidebarHide();
    }
  }

  onSidebarHide() {
    this.visibleChange.emit(false);
    this.submitting = false;
    this.answerForm.reset();
  }
}
