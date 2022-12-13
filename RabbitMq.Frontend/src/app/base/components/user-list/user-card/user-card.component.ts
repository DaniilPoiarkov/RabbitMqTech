import { Component, OnInit, Input } from '@angular/core';
import { CurrentUserService } from 'src/core/services/current-user.service';
import { ToastrNotificationService } from 'src/core/services/toastr-notification.service';
import { User } from 'src/models/user';

@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.sass']
})
export class UserCardComponent implements OnInit {

  constructor(
    private toastr: ToastrNotificationService,
    private currentUserService: CurrentUserService,
  ) { }

  @Input() user: User;

  public currentUser: User

  ngOnInit(): void {
    this.currentUserService.currentUser$.subscribe((user) => {
      this.currentUser = user;
    });
  }

  sendNotification(): void {
    this.toastr.warning('Not implemented yet', 'Warning');
  }

  something(): void {
    this.toastr.warning('Not implemented yet', 'Warning');
  }

}
