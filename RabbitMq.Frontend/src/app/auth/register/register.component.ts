import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/core/services/auth.service';
import { ToastrNotificationService } from 'src/core/services/toastr-notification.service';
import { AccessToken } from 'src/models/access-token';
import { UserRegister } from 'src/models/user-register';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.sass']
})
export class RegisterComponent implements OnInit {

  private userRegister: UserRegister = {};

  public registerForm: FormGroup;
  public nameControl: FormControl;
  public emailControl: FormControl;
  public passwordControl: FormControl;

  private repeatedPassword: string;
  public isErrorDisplay = false;

  get emailError(): string {
    const ctrl = this.emailControl;

    if(ctrl.errors?.['required'] && (!ctrl.touched || !ctrl.dirty)) {
        return 'Email is required';
      }

    return '';
  }

  get nameError(): string {
    const ctrl = this.nameControl;

    if(ctrl.errors?.['required'] && (!ctrl.dirty || !ctrl.touched)) {
      return 'Name is required';
    }

    return '';
  }

  get passwordError(): string {
    const ctrl = this.passwordControl;

    if(ctrl.errors?.['required'] && (!ctrl.dirty || !ctrl.touched)) {
      return 'Password is required';
    }

    if(ctrl.errors?.['minlength'] && (!ctrl.dirty || !ctrl.touched)) {
      return 'Password should be at least 5 characters';
    }

    if(ctrl.value !== this.repeatedPassword) {
      return 'Passwords not match';
    }

    return '';
  }

  constructor(
    private router: Router,
    private authService: AuthService,
    private toastrService: ToastrNotificationService,
  ) { 
    this.emailControl = new FormControl(this.userRegister.email ,[
      Validators.required
    ]);

    this.nameControl = new FormControl(this.userRegister.username, [
      Validators.required
    ]);

    this.passwordControl = new FormControl(this.userRegister.password, [
      Validators.required,
      Validators.minLength(5)
    ]);
  }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      emailControl: this.emailControl,
      nameControl: this.nameControl,
      passwordControl: this.passwordControl
    });
  }

  setRepeatedPassword(val: string): void {
    this.repeatedPassword = val;
  }

  submit(): void {

    if(!this.registerForm.valid || 
      this.passwordControl.value !== this.repeatedPassword) {

      this.registerForm.markAllAsTouched();
      this.toastrService.error('Some values are incorrect', 'Error');
      this.isErrorDisplay = true;
      return;
    }

    this.userRegister = {
      email: this.emailControl.value,
      username: this.nameControl.value,
      password: this.passwordControl.value,
    }

    this.authService.register(this.userRegister).subscribe((resp) => {
      if(resp.ok) {
        this.toastrService.success('Registered successfully!');
        const token = resp.body as AccessToken;
        localStorage.setItem('token', token.token);
        this.router.navigate(['/']);
      }
    }, (err) => {
      this.toastrService.error(err.error.Error, 'Error');
      this.registerForm.reset();
    });

  }

}
