import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CurrentUserService } from 'src/core/services/current-user.service';
import { OpenDialogService } from 'src/core/services/open-dialog.service';
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
  public currectUser: User;

  constructor(
    private service: PrivateNotificationService,
    private toastr: ToastrNotificationService,
    private dialogService: OpenDialogService,
    private currentUserService: CurrentUserService,
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

    this.currentUserService.currentUser$.subscribe((user) => {
      this.currectUser = user;
    });
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
    if(this.notification.sender) {
      this.dialogService.openSendPrivateNotificationDialog(this.currectUser, this.notification.sender);
    } else {
      this.toastr.error('Can\'t send notification to deleted user', 'Error');
    }
  }
}
