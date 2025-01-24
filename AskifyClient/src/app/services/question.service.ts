import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {API_CONSTANTS} from '../constants/api';

export interface Question {
  questionId: string;
  title: string;
  tag: string;
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
  answerId: string;
  content: string;
  createdAt: Date;
  userId: string
}

export type CreateQuestion = {
  title: string;
  content: string;
  tag: number;
}

interface QueryParams {
  pageNumber?: number;
  pageSize?: number;
  search?: string;
}

export type Tag = {
  id: number;
  name: string;
  displayName: string;
}

export type CreateAnswer = {
  content: string;
}

@Injectable({
  providedIn: 'root'
})
export class QuestionService {

  constructor(private http: HttpClient) {
  }

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
      {params: httpParams}
    );
  }


  getQuestionAnswers(questionId: string): Observable<Answer[]> {
    return this.http.get<Answer[]>(API_CONSTANTS.QUESTION.BASE_PATH + `/${questionId}/answers`);
  }

  createQuestion(createQuestion: CreateQuestion): Observable<any> {
    return this.http.post<any>(API_CONSTANTS.QUESTION.BASE_PATH, createQuestion);
  }

  getTags(): Observable<Tag[]> {
    return this.http.get<Tag[]>(API_CONSTANTS.QUESTION.TAGS);
  }

  createAnswer(questionId: string, createAnswer: CreateAnswer): Observable<any> {
    return this.http.post<any>(API_CONSTANTS.QUESTION.BASE_PATH + `/${questionId}/answers`, createAnswer);
  }

  deleteAnswer(questionId: string, answerId: string): Observable<any> {
    return this.http.delete<any>(API_CONSTANTS.QUESTION.BASE_PATH + `/${questionId}/answers/${answerId}`);
  }

  updateAnswer(questionId: string, answerId: string, updateData: { content: string }): Observable<any> {
    return this.http.put(
      `${API_CONSTANTS.QUESTION.BASE_PATH}/${questionId}/answers/${answerId}`,
      updateData
    );
  }
}
