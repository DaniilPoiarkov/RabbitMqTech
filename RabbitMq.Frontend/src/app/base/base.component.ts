import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import { CurrentUserService } from 'src/core/services/current-user.service';
import { ToastrNotificationService } from 'src/core/services/toastr-notification.service';
import { UserService } from 'src/core/services/user.service';
import { environment } from 'src/environments/environment';
import { PrivateNotification } from 'src/models/notifications/private-notification';
import { SimpleNotification } from 'src/models/notifications/simple-notification';
import { User } from 'src/models/user';

@Component({
  selector: 'app-base',
  templateUrl: './base.component.html',
  styleUrls: ['./base.component.sass']
})
export class BaseComponent implements OnInit {

  constructor(
    private router: Router,
    private toastr: ToastrNotificationService,
    private userService: UserService,
    private currentUser: CurrentUserService
  ) { }

  public user: User;

  public notificationConnection: HubConnection;
  public reminderConnection: HubConnection;

  ngOnInit(): void {

    this.currentUser.resetCurrentUser().subscribe();
    this.currentUser.currentUser$.subscribe(user => {
      this.user = user;
    });

    const notificationConnection = new HubConnectionBuilder()
      .withUrl(environment.notificationsHubUrl)
      .withAutomaticReconnect()
      .build();

    this.configureNotificationConnection(notificationConnection);

    notificationConnection.start()
      .catch(() => this.toastr.error(
        'Failed to connect to the server.\n' +
        'Some functionalities can not be processed', 'Error'));

    this.notificationConnection = notificationConnection;

    // const reminderConnection = new HubConnectionBuilder()
    //   .withUrl(environment.reminderHubUrl)
    //   .withAutomaticReconnect()
    //   .build();

    // this.configureReminderConnection(reminderConnection);

    // reminderConnection.start()
    //   .catch(() => this.toastr.error(
    //     'Failed to connect to the server.\n' +
    //     'Some functionalities can not be processed', 'Error'));

    //this.reminderConnection = reminderConnection;

    const el = document.getElementById('avatar-nav-menu') as HTMLElement;
    const menu = document.getElementById('avatar-menu') as HTMLElement;

    el.addEventListener('mouseover', () => {
      menu.style.display = 'block';
    });
    
    menu.addEventListener('mouseleave', () => {
      menu.style.display = 'none';
    })
  }

  configureNotificationConnection(connection: HubConnection): void {

    connection.on('Connected', (connectionId: string) => {
      this.userService.setConnectionId(connectionId)
        .subscribe(() => {
        this.currentUser.resetCurrentUser()
          .subscribe();
      });
    });

    connection.on('PrivateNotification', (notification: PrivateNotification) => { 
      this.toastr.info(
        notification.content, 
        'Recieved new Private Notification! Tap to open page', 
        () => {
          this.router.navigate(['/private-notifications']);
        }, 10000);
    });

    connection.on('SimpleNotification', (notification: SimpleNotification) => { 
      this.toastr.info(
        notification.content, 
        'Recieved new Simple Notification! Tap to open page', 
        () => {
          this.router.navigate(['/simple-notifications']);
        }, 10000);
    });

    connection.on('recieveReminder', (reminder: string) => {
      this.toastr.info(reminder, 'Reminder!');
    });
    
  }

  configureReminderConnection(connection: HubConnection): void {
    connection.on('recieveReminder', (reminder: string) => {
      this.toastr.info(reminder, 'Reminder!');
    });
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
