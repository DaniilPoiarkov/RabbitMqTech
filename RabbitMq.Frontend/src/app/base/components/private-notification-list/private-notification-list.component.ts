import { Component, OnInit, Input } from '@angular/core';
import { CurrentUserService } from 'src/core/services/current-user.service';
import { PrivateNotificationService } from 'src/core/services/private-notification.service';
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
    private toastr: ToastrNotificationService,
    private service: PrivateNotificationService
  ) { }

  public notifications: PrivateNotification[];

  public user: User;

  public loaded = false;

  ngOnInit(): void {
    this.currentUser.currentUser$.subscribe((user) => {
      this.user = user;
      this.getNotifications();
    });

  }

  private getNotifications(): void {

    this.service.getAllNotifications(this.user.id).subscribe((resp) => {
      this.notifications = resp.body as PrivateNotification[];
      this.loaded = true;
    }, (err) => {
      this.toastr.error(err.error.Error, 'Error');
    });

  }

  removeNotification(id: number): void {
    const index = this.notifications.findIndex((n) => n.id == id);
    this.notifications.splice(index, 1);
  }

}
