import { Component, OnInit, Input } from '@angular/core';
import { CurrentUserService } from 'src/core/services/current-user.service';
import { ToastrNotificationService } from 'src/core/services/toastr-notification.service';
import { PrivateNotification } from 'src/models/notifications/private-notification';
import { User } from 'src/models/user';

@Component({
  selector: 'app-private-notification-list',
  templateUrl: './private-notification-list.component.html',
  styleUrls: ['./private-notification-list.component.sass']
})
export class PrivateNotificationListComponent implements OnInit {

  constructor(
    private currentUser: CurrentUserService,
    private toastr: ToastrNotificationService
  ) { }

  @Input() notifications: PrivateNotification[];

  public user: User;

  ngOnInit(): void {
    this.currentUser.currentUser$.subscribe((user) => {
      this.user = user;
      console.log(user);
    });
    
  }

}
