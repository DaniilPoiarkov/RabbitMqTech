import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { CurrentUserService } from 'src/core/services/current-user.service';
import { ToastrNotificationService } from 'src/core/services/toastr-notification.service';
import { User } from 'src/models/user';

@Component({
  selector: 'app-avatar',
  templateUrl: './avatar.component.html',
  styleUrls: ['./avatar.component.sass']
})
export class AvatarComponent implements OnInit {

  constructor(
    private currentUser: CurrentUserService,
    private router: Router,
    private toastr: ToastrNotificationService
  ) { }

  public user$: Observable<User>;
  public avatarMock: string = 'https://burst.shopifycdn.com/photos/person-holds-a-book-over-a-stack-and-turns-the-page.jpg?width=1200&format=pjpg&exif=0&iptc=0';// = '';

  ngOnInit(): void {
    this.user$ = this.currentUser.currentUser$;
  }

  logout(): void {
    localStorage.clear();
    this.toastr.success('Logout successfull');
    this.router.navigate(['/auth/login']);
  }

  profile(): void {
    this.router.navigate(['/profile']);
  }

}
