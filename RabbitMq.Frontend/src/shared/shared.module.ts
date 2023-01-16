import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputComponent } from './input/input.component';
import { ButtonComponent } from './button/button.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SendPrivateNotificationDialogComponent } from './send-private-notification-dialog/send-private-notification-dialog.component';
import { SendSimpleNotificationDialogComponent } from './send-simple-notification-dialog/send-simple-notification-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { AvatarComponent } from './avatar/avatar.component';



@NgModule({
  declarations: [
    InputComponent,
    ButtonComponent,
    SendPrivateNotificationDialogComponent,
    SendSimpleNotificationDialogComponent,
    AvatarComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
  ],
  exports: [
    InputComponent,
    ButtonComponent,
    AvatarComponent
  ]
})
export class SharedModule { }

