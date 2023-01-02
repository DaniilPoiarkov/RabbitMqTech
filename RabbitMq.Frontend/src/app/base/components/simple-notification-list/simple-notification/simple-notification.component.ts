import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SimpleNotificationService } from 'src/core/services/simple-notification.service';
import { ToastrNotificationService } from 'src/core/services/toastr-notification.service';
import { SimpleNotification } from 'src/models/notifications/simple-notification';

@Component({
  selector: 'app-simple-notification',
  templateUrl: './simple-notification.component.html',
  styleUrls: ['./simple-notification.component.sass']
})
export class SimpleNotificationComponent implements OnInit {

  @Input() notification: SimpleNotification;

  @Output() deleted = new EventEmitter<number>();

  constructor(
    private service: SimpleNotificationService,
    private toastr: ToastrNotificationService
  ) { }

  ngOnInit(): void {

  }

  deleteNotification(): void {
    this.service.deleteNotification(this.notification.id).subscribe(() => {
      this.toastr.success('Notification has been deleted successfully', 'Success!');
      this.deleted.emit(this.notification.id);
    }, (err) => {
      this.toastr.error(err.error.Error, 'Error');
    });
  }

}
