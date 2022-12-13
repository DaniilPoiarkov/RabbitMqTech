import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import { CurrentUserService } from 'src/core/services/current-user.service';
import { ToastrNotificationService } from 'src/core/services/toastr-notification.service';
import { UserService } from 'src/core/services/user.service';
import { environment } from 'src/environments/environment';
import { PrivateNotification } from 'src/models/notifications/private-notification';
import { SimpleNotification } from 'src/models/notifications/simple-notification';

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

  public privateNotifications: PrivateNotification[] = [];
  public simpleNotifications: SimpleNotification[] = [];

  public hubConnection: HubConnection;

  ngOnInit(): void {

    this.currentUser.resetCurrentUser().subscribe();

    const connection = new HubConnectionBuilder()
      .withUrl(environment.hubUrl)
      .withAutomaticReconnect()
      .build();

    this.configureConnection(connection);

    connection.start()
      .catch(() => this.toastr.error(
        'Failed to connect to the server.\n' +
        'Some functionalities can not be processed', 'Error'));

    this.hubConnection = connection;
  }

  configureConnection(connection: HubConnection): void {

    connection.on('Connected', (connectionId: string) => {
      this.userService.setConnectionId(connectionId)
        .subscribe(() => {
        this.currentUser.resetCurrentUser()
          .subscribe();
      });
    });

    connection.on('PrivateNotification', (notification: PrivateNotification) => { 
      console.log(notification);
      this.privateNotifications.push(notification); 
    });

    connection.on('SimpleNotification', (notification: SimpleNotification) => { 
      console.log(notification);
      this.simpleNotifications.push(notification);
    });
  }

  goToPrivateNotificationsPage(): void {
    this.router.navigate(['/private']);
  }

  goToSimpleNotificationsPage(): void {
    this.router.navigate(['/simple']);
  }

  goToAllUsersPage(): void {
    this.router.navigate(['/users']);
  }

  logout(): void {
    localStorage.clear();
    this.toastr.success('Logout successfull');
    this.router.navigate(['auth/login']);
  }

}
