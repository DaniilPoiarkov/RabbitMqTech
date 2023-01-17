import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { AuthService } from 'src/core/services/auth.service';
import { CurrentUserService } from 'src/core/services/current-user.service';
import { ToastrNotificationService } from 'src/core/services/toastr-notification.service';
import { UserService } from 'src/core/services/user.service';
import { ResetPasswordModel } from 'src/models/reset-password-model';
import { User } from 'src/models/user';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.sass']
})
export class UserProfileComponent implements OnInit {

  public user$: Observable<User>;
  public resetPasswordModel: ResetPasswordModel = { };
  public passwordControl: FormControl;
  public displayError = false;
  public repeatedPassword: string;
  public avatarUrl: string;

  constructor(
    private currentUser: CurrentUserService,
    private authService: AuthService,
    private userService: UserService,
    private toastr: ToastrNotificationService
  ) { }

  ngOnInit(): void {
    this.user$ = this.currentUser.currentUser$;
    this.passwordControl = new FormControl(this.resetPasswordModel.newPassword, [
      Validators.required,
      Validators.minLength(5)
    ]);
  }

  get errorMessage(): string {
    const ctrl = this.passwordControl;
    if(ctrl.errors?.['required'] && (ctrl.touched || ctrl.dirty)) {
      return 'New password required to be updated';
    }
    if(ctrl.errors?.['minlength'] && (ctrl.touched || ctrl.dirty)) {
      return 'Minimum length is 5 chars';
    }
    return '';
  }

  get repeatedPasswordErrorMessage(): string {
    if(this.repeatedPassword != this.passwordControl.value) {
      return 'Password not match';
    }
    return '';
  }

  setRepeatedPassword(password: string): void {
    this.repeatedPassword = password;
  }

  resetPassword(): void {

    if(this.passwordControl.invalid || this.repeatedPassword != this.passwordControl.value) {
      this.toastr.error('Some values are incorrect');
      this.displayError = true;
      return;
    }

    this.user$.subscribe(user => {
      this.resetPasswordModel = {
        newPassword: this.passwordControl.value,
        email: user.email
      }
      this.sendRequest();
    });

  }

  setAvatarUrl(url: string): void {
    this.avatarUrl = url;
  }
  
  private sendRequest(): void {
    this.authService.resetPassword(this.resetPasswordModel).subscribe(resp => {
      if(resp.ok) {
        this.toastr.success('Your password updated successfully');
      }
    });
  }

  public updateAvatarUrl(): void {
    this.user$.subscribe(user => {
      this.userService.updateAvatar(user.id, this.avatarUrl).subscribe(resp => {
        if(resp.ok) {
          this.toastr.success('Avatar updated successfully!');
        }
      });
    });
  }

  public removeAvatarUrl(): void {
    this.user$.subscribe(user => {
      this.userService.removeAvatar(user.id).subscribe(() => {
        this.toastr.success('Avatar removed successfuly!');
      });
    });
  }

}
