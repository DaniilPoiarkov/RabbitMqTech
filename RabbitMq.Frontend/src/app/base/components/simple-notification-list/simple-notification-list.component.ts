import { Component, OnInit } from '@angular/core';
import { CurrentUserService } from 'src/core/services/current-user.service';
import { SimpleNotificationService } from 'src/core/services/simple-notification.service';
import { ToastrNotificationService } from 'src/core/services/toastr-notification.service';
import { SimpleNotification } from 'src/models/notifications/simple-notification';
import { User } from 'src/models/user';

@Component({
  selector: 'app-simple-notification-list',
  templateUrl: './simple-notification-list.component.html',
  styleUrls: ['./simple-notification-list.component.sass']
})
export class SimpleNotificationListComponent implements OnInit {

  public notifications: SimpleNotification[] = [];
  public user: User;

  constructor(
    private service: SimpleNotificationService,
    private toastr: ToastrNotificationService,
    private currentUser: CurrentUserService,
  ) { }

  ngOnInit(): void {
    this.currentUser.currentUser$.subscribe((user) => {
      this.user = user;
      this.getNotifications();
    });

  }

  private getNotifications(): void {
    this.service.getAllNotifications(this.user.id).subscribe((resp) => {
      this.notifications = resp.body as SimpleNotification[];
    }, (err) => {
      this.toastr.error(err.error.Error, 'Error');
    });
  }

  removeNotification(id: number): void {
    const index = this.notifications.findIndex((n) => n.id == id);
    this.notifications.splice(index, 1);
  }

}
