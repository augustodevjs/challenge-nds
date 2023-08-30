import { Observable, fromEvent, merge } from 'rxjs';
import { FormGroup, Validators, FormBuilder, FormControlName } from '@angular/forms'
import { Component, OnInit, AfterViewInit, ViewChildren, ElementRef } from '@angular/core';
import { DisplayMessage, GenericValidator, LoginFormModel, loginFormMessages } from '../../../shared';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit, AfterViewInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];

  loginForm!: FormGroup;
  LoginFormModel: LoginFormModel = {} as LoginFormModel;

  genericValidator: GenericValidator;
  displayMessage: DisplayMessage = {}

  constructor(private builder: FormBuilder, private loginFormMessages: loginFormMessages) {
    this.genericValidator = new GenericValidator(this.loginFormMessages.validationMessages);
  }

  ngOnInit(): void {
    this.loginForm = this.builder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    })
  }

  ngAfterViewInit<T>(): void {
    let controlBlurs: Observable<T>[] = this.formInputElements.map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));

    merge(...controlBlurs).subscribe(() => {
      this.displayMessage = this.genericValidator.processMessages(this.loginForm)
    })
  }

  login(): void {
    this.LoginFormModel = Object.assign({}, this.LoginFormModel, this.loginForm.value);

    console.log(this.LoginFormModel)
  }
}
