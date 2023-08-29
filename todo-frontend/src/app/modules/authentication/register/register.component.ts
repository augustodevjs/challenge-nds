import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormGroup, Validators, FormBuilder, FormControlName } from '@angular/forms'
import { ConfirmedValidator, DisplayMessage, GenericValidator, RegisterFormModel, registerFormMessages } from '../../../shared';
import { Observable, fromEvent, merge } from 'rxjs';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  providers: [registerFormMessages]
})
export class RegisterComponent implements OnInit, AfterViewInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];

  registerForm!: FormGroup;
  genericValidator: GenericValidator;
  displayMessage: DisplayMessage = {}
  registerFormModel: RegisterFormModel = {} as RegisterFormModel;

  constructor(private builder: FormBuilder, private registerFormMessages: registerFormMessages) {
    this.genericValidator = new GenericValidator(this.registerFormMessages.validationMessages);
  }

  ngOnInit(): void {
    this.registerForm = this.builder.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      passwordConfirm: ['', Validators.required,]
    }, {
      validators: [
        ConfirmedValidator('password', 'passwordConfirm')
      ]
    })
  }

  ngAfterViewInit<T>(): void {
    let controlBlurs: Observable<T>[] = this.formInputElements.map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));

    merge(...controlBlurs).subscribe(() => {
      this.displayMessage = this.genericValidator.processMessages(this.registerForm)
    })
  }

  register(): void {
    this.registerFormModel = Object.assign({}, this.registerFormModel, this.registerForm.value);

    console.log(this.registerFormModel)
  }
}
