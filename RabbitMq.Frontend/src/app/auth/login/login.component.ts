import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/core/services/auth.service';
import { ToastrNotificationService } from 'src/core/services/toastr-notification.service';
import { AccessToken } from 'src/models/access-token';
import { UserLogin } from 'src/models/user-login';
import { InputComponent } from 'src/shared/input/input.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.sass']
})
export class LoginComponent implements OnInit {

  private userLogin: UserLogin = {};

  public loginForm: FormGroup;
  public emailControl: FormControl;
  public passwordControl: FormControl;
  public isErrorDisplay = false;

  @ViewChild('passwordInput') passwordInput: InputComponent;
  @ViewChild('emailInput') emailInput: InputComponent;

  constructor(
    private router: Router,
    private authService: AuthService,
    private toastrService: ToastrNotificationService
  ) { 
    this.emailControl = new FormControl(this.userLogin.email, [
      Validators.required,
    ]);

    this.passwordControl = new FormControl(this.userLogin.password, [
      Validators.required,
      Validators.minLength(5),
    ]);
  }

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      emailControl: this.emailControl,
      passwordControl: this.passwordControl
    });
  }

  get emailError(): string {
    const ctrl = this.emailControl;

    if(ctrl.errors?.['required'] && (ctrl.dirty || ctrl.touched)) {
      return 'Email is required';
    }

    return '';
  }

  get passwordError(): string {
    const ctrl = this.passwordControl;

    if(ctrl.errors?.['required'] && (ctrl.dirty || ctrl.touched)) {
      return 'Password is required';
    }

    if(ctrl.errors?.['minlength'] && (ctrl.dirty || ctrl.touched)) {
      return 'Password must be at least 5 characters';
    }

    return '';
  }

  public submit(): void {

    this.userLogin = {
      email: this.emailControl.value,
      password: this.passwordControl.value,
    }

    if(!this.loginForm.valid) {
      this.toastrService.error('Invalid values');
      this.loginForm.markAllAsTouched(); 
      this.isErrorDisplay = true;
      return;
    }

    if(!this.loginForm.dirty || !this.loginForm.touched) {
      this.toastrService.error('Empty values', 'Error');
      this.loginForm.markAllAsTouched();
      this.isErrorDisplay = true;
      return;
    }

    this.authService.login(this.userLogin).subscribe((resp) => {
      if(resp.ok) {
        const token = resp.body as AccessToken;
        
        localStorage.setItem('token', token.token);
        this.toastrService.success('Login successfull');
        this.router.navigate(['/']);
      }
    }, (err) => {
      this.loginForm.reset();
      this.toastrService.error(err.error.Error);
    });
  }

}
