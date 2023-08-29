import { Injectable } from "@angular/core";
import { ValidationMessages } from "..";

@Injectable()
export class loginFormMessages {
  validationMessages: ValidationMessages = {
    email: {
      required: 'O nome é obrigatório',
      email: 'O email não está no formato correto'
    },
    password: {
      required: 'A senha é obrigatório',
    }
  }
}
