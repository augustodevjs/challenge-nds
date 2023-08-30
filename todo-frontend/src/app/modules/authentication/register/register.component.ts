import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormGroup, Validators, FormBuilder, FormControlName } from '@angular/forms'
import { ConfirmedValidator, DisplayMessage, GenericValidator, RegisterFormModel, RegisterFormResponse, RegisterService, registerFormMessages } from '../../../shared';
import { Observable, fromEvent, merge } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit, AfterViewInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];

  isLoading: boolean = false;
  registerForm!: FormGroup;
  genericValidator: GenericValidator;
  displayMessage: DisplayMessage = {}
  registerFormModel: RegisterFormModel = {} as RegisterFormModel;

  constructor(private builder: FormBuilder, private registerFormMessages: registerFormMessages, private registerService: RegisterService) {
    this.genericValidator = new GenericValidator(this.registerFormMessages.validationMessages);
  }

  ngOnInit(): void {
    this.registerForm = this.builder.group({
      name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(150)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(250)]],
      confirmPassword: ['', [Validators.required]]
    }, {
      validators: [
        ConfirmedValidator('password', 'confirmPassword'),
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
    this.isLoading = true;

    this.registerFormModel = Object.assign({}, this.registerFormModel, this.registerForm.value);

    this.registerService.register(this.registerFormModel).subscribe({
      next: (data: RegisterFormResponse) => {
        console.log('Usuario cadastrado', data);
      },
      error: (error: HttpErrorResponse) => {
        this.isLoading = false;
        this.registerService.errorHandling(error)
      },
      complete: () => {
        this.isLoading = false;
      }
    })
  }
}
