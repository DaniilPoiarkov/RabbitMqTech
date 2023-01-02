import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { PrivateNotificationService } from 'src/core/services/private-notification.service';
import { ToastrNotificationService } from 'src/core/services/toastr-notification.service';
import { User } from 'src/models/user';

@Component({
  selector: 'app-send-private-notification-dialog',
  templateUrl: './send-private-notification-dialog.component.html',
  styleUrls: ['./send-private-notification-dialog.component.sass']
})
export class SendPrivateNotificationDialogComponent {

  constructor(
    public dialogRef: MatDialogRef<SendPrivateNotificationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { sender: User, reciever: User},
    private toastr: ToastrNotificationService,
    private service: PrivateNotificationService,
  ) { }

  private content: string;

  send(): void {
    
    this.service.sendNotification({
      content: this.content,
      recieverId: this.data.reciever.id,
      senderId: this.data.sender.id,
      id: 0,
    }).subscribe(() => {
      this.toastr.success('Send successfully!', 'Success');
      this.dialogRef.close();
    }, (err) => {
      this.toastr.error(err.error.Error, 'Error');
    });
  }

  setContent(val: string): void {
    this.content = val;
  }

}
