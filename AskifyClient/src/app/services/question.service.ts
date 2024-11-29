import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {API_CONSTANTS} from '../constants/api';

export interface Question {
  questionId: string;
  title: string;
  createdAt: Date;
  userId: string;
}

export interface Answer {
  questionId: string;
  content: string;
  createdAt: Date;
  userId: string
}

export type CreateQuestion = {
  title: string;
  content: string;
}

@Injectable({
  providedIn: 'root'
})
export class QuestionService {

  constructor(private http: HttpClient) {}

  getQuestions(): Observable<Question[]> {
    return this.http.get<Question[]>(API_CONSTANTS.QUESTION.BROWSE);
  }


  getQuestionAnswers(questionId: string): Observable<Answer[]> {
    return this.http.get<Answer[]>(API_CONSTANTS.QUESTION.BASE_PATH + `/${questionId}/answers`);
  }

  createQuestion(createQuestion: CreateQuestion) : Observable<any> {
    return this.http.post<any>(API_CONSTANTS.QUESTION.BASE_PATH, createQuestion);
  }
}
