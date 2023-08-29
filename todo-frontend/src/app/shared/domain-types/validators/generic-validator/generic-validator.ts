import { FormGroup } from '@angular/forms';
import { DisplayMessage, ValidationMessages } from '../../types';

export class GenericValidator {
  constructor(private validationMessages: ValidationMessages) { }

  processMessages(container: FormGroup): DisplayMessage {
    const messages: DisplayMessage = {};

    Object.keys(container.controls).forEach((controlKey) => {
      const control = container.controls[controlKey];

      if (control instanceof FormGroup) {
        const childMessages = this.processMessages(control);
        Object.assign(messages, childMessages);
      } else {
        if (this.validationMessages[controlKey]) {
          messages[controlKey] = '';

          if ((control.dirty || control.touched) && control.errors) {
            Object.keys(control.errors).forEach((errorKey) => {
              const errorMessage = this.validationMessages[controlKey][errorKey];
              if (errorMessage) {
                messages[controlKey] += errorMessage;
              }
            });
          }
        }
      }
    });

    return messages;
  }
}
