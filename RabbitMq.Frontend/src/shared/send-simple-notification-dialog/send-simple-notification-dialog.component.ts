import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SimpleNotificationService } from 'src/core/services/simple-notification.service';
import { ToastrNotificationService } from 'src/core/services/toastr-notification.service';
import { User } from 'src/models/user';

@Component({
  selector: 'app-send-simple-notification-dialog',
  templateUrl: './send-simple-notification-dialog.component.html',
  styleUrls: ['./send-simple-notification-dialog.component.sass']
})
export class SendSimpleNotificationDialogComponent {

  constructor(
    public dialogRef: MatDialogRef<SendSimpleNotificationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { reciever: User},
    private toastr: ToastrNotificationService,
    private service: SimpleNotificationService,
  ) { }

  private content: string;
  
  send(): void {
    
    this.service.sendNotification({
      content: this.content,
      recieverId: this.data.reciever.id,
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
