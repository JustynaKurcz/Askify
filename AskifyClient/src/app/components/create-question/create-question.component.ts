import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import {Button, ButtonDirective} from 'primeng/button';
import { NgIf } from '@angular/common';
import {CreateQuestion, QuestionService, Tag} from '../../services/question.service';
import { DropdownModule } from 'primeng/dropdown';

@Component({
  selector: 'app-create-question',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    InputTextModule,
    InputTextareaModule,
    Button,
    NgIf,
    DropdownModule,
    ButtonDirective
  ],
  templateUrl: './create-question.component.html',
  styleUrl: './create-question.component.css'
})
export class CreateQuestionComponent implements OnInit {
  questionForm: FormGroup;
  submitting = false;
  tags: Tag[] = [];

  constructor(
      private fb: FormBuilder,
      private ref: DynamicDialogRef,
      private questionService: QuestionService
  ) {
    this.questionForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(250)]],
      content: ['', [Validators.required, Validators.maxLength(5000)]],
      tagId: [null, Validators.required]
    });
  }

  ngOnInit() {
    this.loadTags();
  }

  loadTags() {
    this.questionService.getTags().subscribe({
      next: (tags) => {
        this.tags = tags;
      },
      error: (error) => {
        console.error('Error loading tags:', error);
      }
    });
  }

  onSubmit() {
    if (this.questionForm.valid) {
      const newQuestion = this.questionForm.value;
      this.submitting = true;
      this.questionService.createQuestion(newQuestion as CreateQuestion)
          .subscribe({
            next: () => {
              this.ref.close(newQuestion);
            },
            error: () => {
              this.submitting = false;
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
