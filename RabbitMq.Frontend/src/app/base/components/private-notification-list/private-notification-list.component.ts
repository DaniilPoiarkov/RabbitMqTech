import { Component, OnInit } from '@angular/core';
import { map, Observable } from 'rxjs';
import { CurrentUserService } from 'src/core/services/current-user.service';
import { PrivateNotificationService } from 'src/core/services/private-notification.service';
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
    private service: PrivateNotificationService
  ) { }

  public notifications$: Observable<PrivateNotification[]>;

  public user: User;

  ngOnInit(): void {
    this.currentUser.currentUser$.subscribe((user) => {
      this.user = user;
      this.getNotifications();
    });

    const el = document.getElementById('private') as HTMLElement;
    el.style.color = '#45a29e';
    el.style.margin = '2px 0 0 0';
    el.style.borderBottom = '2px solid #66fcf1';
  }

  private getNotifications(): void {
    this.notifications$ = this.service.getAllNotifications(this.user.id).pipe(
      map(resp => {
        return resp.body as PrivateNotification[];
      })
    )
  }

  removeNotification(): void {
    this.getNotifications();
  }

}
