import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputComponent } from './input/input.component';
import { ButtonComponent } from './button/button.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SendPrivateNotificationDialogComponent } from './send-private-notification-dialog/send-private-notification-dialog.component';
import { SendSimpleNotificationDialogComponent } from './send-simple-notification-dialog/send-simple-notification-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';



@NgModule({
  declarations: [
    InputComponent,
    ButtonComponent,
    SendPrivateNotificationDialogComponent,
    SendSimpleNotificationDialogComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
  ],
  exports: [
    InputComponent,
    ButtonComponent
  ]
})
export class SharedModule { }

