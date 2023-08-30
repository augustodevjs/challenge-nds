import { Injectable } from "@angular/core";
import { ValidationMessages } from "..";

@Injectable({
  providedIn: 'root'
})
export class registerFormMessages {
  validationMessages: ValidationMessages = {
    name: {
      required: 'O nome é obrigatório'
    },
    email: {
      required: 'O nome é obrigatório',
      email: 'O email não está no formato correto'
    },
    password: {
      required: 'A senha é obrigatória',
    },
    passwordConfirm: {
      required: 'A confirmação de senha é obrigatória',
      confirmedValidator: 'As Senhas não conferem.'
    }
  }
}
