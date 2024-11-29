import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {API_CONSTANTS} from '../constants/api';

export interface Question {
  questionId: string;
  title: string;
  createdAt: Date;
  userId: string;
}

export type PaginationQuestion = {
  items: Question[];
  totalItems: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;

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

interface QueryParams {
  pageNumber?: number;
  pageSize?: number;
  search?: string;
}

@Injectable({
  providedIn: 'root'
})
export class QuestionService {

  constructor(private http: HttpClient) {}

  getQuestions(params: QueryParams = {}): Observable<PaginationQuestion> {
    let httpParams = new HttpParams();

    if (params.pageNumber) {
      httpParams = httpParams.set('pageNumber', params.pageNumber.toString());
    }
    if (params.pageSize) {
      httpParams = httpParams.set('pageSize', params.pageSize.toString());
    }
    if (params.search) {
      httpParams = httpParams.set('search', params.search);
    }

    return this.http.get<PaginationQuestion>(
      API_CONSTANTS.QUESTION.BASE_PATH,
      { params: httpParams }
    );
  }


  getQuestionAnswers(questionId: string): Observable<Answer[]> {
    return this.http.get<Answer[]>(API_CONSTANTS.QUESTION.BASE_PATH + `/${questionId}/answers`);
  }

  createQuestion(createQuestion: CreateQuestion) : Observable<any> {
    return this.http.post<any>(API_CONSTANTS.QUESTION.BASE_PATH, createQuestion);
  }
}
