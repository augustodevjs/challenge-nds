import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormGroup, Validators, FormBuilder, FormControlName } from '@angular/forms'
import { ConfirmedValidator, DisplayMessage, GenericValidator, RegisterFormModel, RegisterFormResponse, RegisterService, registerFormMessages } from '../../../shared';
import { Observable, fromEvent, merge } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit, AfterViewInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];

  registerForm!: FormGroup;
  errors: string[] = ['asdaaskdjalksdjalksjdalksjdlkajsdlkajsdklasjlkakjsdklajss']
  isLoading: boolean = false;
  genericValidator: GenericValidator;
  displayMessage: DisplayMessage = {}
  registerFormModel: RegisterFormModel = {} as RegisterFormModel;

  constructor(
    private router: Router,
    private builder: FormBuilder,
    private _snackBar: MatSnackBar,
    private registerService: RegisterService,
    private registerFormMessages: registerFormMessages,
  ) {
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

  openSnackBar(message: string,) {
    this._snackBar.open(message, 'Ok', {
      horizontalPosition: 'end',
      verticalPosition: 'top',
    });
  }

  register(): void {
    this.isLoading = true;

    this.registerFormModel = Object.assign({}, this.registerFormModel, this.registerForm.value);

    this.registerService.register(this.registerFormModel).subscribe({
      next: (data: RegisterFormResponse) => {
        this.openSnackBar(`Usuário ${data.name} foi criado com sucesso`);
        this.router.navigateByUrl('/auth/login')
      },
      error: (error: HttpErrorResponse) => {
        this.isLoading = false;
        this.errors = this.registerService.errorHandling(error)
      },
      complete: () => {
        this.isLoading = false;
      }
    })
  }
}
