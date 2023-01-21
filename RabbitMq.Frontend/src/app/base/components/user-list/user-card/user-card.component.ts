import { Component, OnInit, Input } from '@angular/core';
import { CurrentUserService } from 'src/core/services/current-user.service';
import { OpenDialogService } from 'src/core/services/open-dialog.service';
import { User } from 'src/models/user';

@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.sass']
})
export class UserCardComponent implements OnInit {

  constructor(
    private currentUserService: CurrentUserService,
    private dialogService: OpenDialogService
  ) { }

  @Input() user: User;

  public currentUser: User

  ngOnInit(): void {
    this.currentUserService.currentUser$.subscribe((user) => {
      this.currentUser = user;
    });
  }

  sendPrivateNotification(): void {
    this.dialogService.openSendPrivateNotificationDialog(this.currentUser, this.user);
  }

  sendSimpleNotification(): void {
    this.dialogService.openSendSimpleNotificationDialog(this.user);
  }

  onAvatarClick(): void {
    //TODO: Implement user-pages and set here navigate to their pages
    console.log(this.user);
  }

}
