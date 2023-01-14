import { Component, OnInit } from '@angular/core';
import { map, Observable } from 'rxjs';
import { CurrentUserService } from 'src/core/services/current-user.service';
import { SimpleNotificationService } from 'src/core/services/simple-notification.service';
import { SimpleNotification } from 'src/models/notifications/simple-notification';
import { User } from 'src/models/user';

@Component({
  selector: 'app-simple-notification-list',
  templateUrl: './simple-notification-list.component.html',
  styleUrls: ['./simple-notification-list.component.sass']
})
export class SimpleNotificationListComponent implements OnInit {

  public notifications$: Observable<SimpleNotification[]>;
  public user: User;

  constructor(
    private service: SimpleNotificationService,
    private currentUser: CurrentUserService,
  ) { }

  ngOnInit(): void {
    this.currentUser.currentUser$.subscribe((user) => {
      this.user = user;
      this.getNotifications();
    });
    
    const el = document.getElementById('simple') as HTMLElement;
    el.style.color = '#45a29e';
    el.style.margin = '2px 0 0 0';
    el.style.borderBottom = '2px solid #66fcf1';
  }

  private getNotifications(): void {
    this.notifications$ = this.service.getAllNotifications(this.user.id).pipe(
      map(resp => resp.body as SimpleNotification[])
    );
  }

  removeNotification(): void {
    this.getNotifications();
  }

}
