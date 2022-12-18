import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { User } from 'src/models/user';
import { SendPrivateNotificationDialogComponent } from 'src/shared/send-private-notification-dialog/send-private-notification-dialog.component';
import { SendSimpleNotificationDialogComponent } from 'src/shared/send-simple-notification-dialog/send-simple-notification-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class OpenDialogService {

  constructor(private matDialog: MatDialog) { }

  openSendPrivateNotificationDialog(sender: User, reciever: User): void {
    const dialog = this.matDialog.open(SendPrivateNotificationDialogComponent, {
      data: {
        sender: sender,
        reciever: reciever
      }
    });

    dialog.afterClosed();
  }

  openSendSimpleNotificationDialog(reciever: User): void {
    const dialog = this.matDialog.open(SendSimpleNotificationDialogComponent, {
      data: {
        reciever: reciever,
      }
    });

    dialog.afterClosed();
  }
}
