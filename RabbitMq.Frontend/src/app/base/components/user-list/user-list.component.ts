import { Component, OnInit } from '@angular/core';
import { CurrentUserService } from 'src/core/services/current-user.service';
import { ToastrNotificationService } from 'src/core/services/toastr-notification.service';
import { UserService } from 'src/core/services/user.service';
import { User } from 'src/models/user';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.sass']
})
export class UserListComponent implements OnInit {

  constructor(
    private userService: UserService,
    private toastr: ToastrNotificationService,
    private currentUser: CurrentUserService,
  ) { }

  public user: User;
  public allUsers: User[] = [];

  ngOnInit(): void {
    
    this.currentUser.currentUser$.subscribe((user) => {
      this.user = user;
    });

    this.userService.getAllUsers().subscribe((resp) => {
      const users = resp.body as User[];
      this.allUsers = users;
    });

  }

}
