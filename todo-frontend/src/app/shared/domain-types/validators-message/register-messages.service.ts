import { Injectable } from "@angular/core";
import { ValidationMessages } from "..";

@Injectable({
  providedIn: 'root'
})
export class registerFormMessages {
  validationMessages: ValidationMessages = {
    name: {
      required: 'O nome é obrigatório',
      minlength: 'O nome deve ter no mínimo 3 caracteres',
      maxlength: 'O nome deve ter no máximo 150 caracteres',
    },
    email: {
      required: 'O nome é obrigatório',
      email: 'O email não está no formato correto',
    },
    password: {
      required: 'A senha é obrigatória',
      minlength: 'O nome deve ter no mínimo 3 caracteres',
      maxlength: 'O nome deve ter no máximo 250 caracteres',
    },
    passwordConfirm: {
      required: 'A confirmação de senha é obrigatória',
      confirmedvalidator: 'As Senhas não conferem',
    }
  }
}
