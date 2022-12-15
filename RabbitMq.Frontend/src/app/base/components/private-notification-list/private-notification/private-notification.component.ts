import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { PrivateNotificationService } from 'src/core/services/private-notification.service';
import { ToastrNotificationService } from 'src/core/services/toastr-notification.service';
import { PrivateNotification } from 'src/models/notifications/private-notification';
import { User } from 'src/models/user';

@Component({
  selector: 'app-private-notification',
  templateUrl: './private-notification.component.html',
  styleUrls: ['./private-notification.component.sass']
})
export class PrivateNotificationComponent implements OnInit {

  @Input() notification: PrivateNotification;

  @Output() deleted = new EventEmitter<number>();

  public sender: User;

  constructor(
    private service: PrivateNotificationService,
    private toastr: ToastrNotificationService
  ) { }

  ngOnInit(): void {
    if(this.notification.sender) {
      this.sender = this.notification.sender;
    } else {
      this.sender = {
        id: -1,
        username: 'Deleted user',
        email: '',
        connectionId: '',
        password: '',
      }
    }
  }

  public deleteNotification(): void {
    this.service.deleteNotification(this.notification.id).subscribe(() => {
      this.toastr.success('Notification has been deleted', 'Success!');
      this.deleted.emit(this.notification.id);
    }, (err) => {
      this.toastr.error(err.error.Error, 'Error');
    });
  }

  reply(): void {
    this.toastr.warning('Not implemented yet', 'Warning!');
  }
}
