import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { User } from 'src/models/user';
import { ToastrNotificationService } from './toastr-notification.service';
import { UserService } from './user.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class CurrentUserService {

  constructor(
    private userService: UserService,
    private toastr: ToastrNotificationService
  ) { }

  private currentUser = new ReplaySubject<User>(1);

  public currentUser$ = this.currentUser.asObservable();

  private jwtHelper = new JwtHelperService();

  public resetCurrentUser(): Observable<void> {
    return new Observable(() => {
      this.userService.getCurrentUser().subscribe((resp) => {
        const user = resp.body as User;
        this.currentUser.next(user);
      }, (err) => {
        this.toastr.error(
          err.error.Error + ' Your data might not be saved.\n' +
          'Tap to try again', 
          'Error',
          () => this.resetCurrentUser())
      });
    });
  }

  public getUserId(): number | null {
    
    const token = localStorage.getItem('token');

    if(token) {
      const decoded = this.jwtHelper.decodeToken(token);
      return decoded.id;
    }

    return null;
  }
}
