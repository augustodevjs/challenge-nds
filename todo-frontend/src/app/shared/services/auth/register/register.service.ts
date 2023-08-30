import { Observable } from "rxjs";
import { Injectable } from '@angular/core';
import { HttpErrorResponse } from "@angular/common/http";
import { BaseService } from '../../base-service/base-service';
import { RegisterFormModel, RegisterFormResponse, HttpStatusCode, UnexpectedError, ValidationError } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class RegisterService extends BaseService {
  register(user: RegisterFormModel): Observable<RegisterFormResponse> {
    const url = `${this.apiUrl}/auth/register`;
    return this.httpClient.post<RegisterFormResponse>(url, user);
  }

  errorHandling(response: HttpErrorResponse) {
    switch (response.status) {
      case HttpStatusCode.BadRequest:
        console.log(response.error.erros[0])
        throw new ValidationError(response.error.erros[0]);
      default:
        throw new UnexpectedError();
    }
  }
}
