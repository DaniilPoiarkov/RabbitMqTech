import { Component, OnInit } from '@angular/core';
import { map, Observable } from 'rxjs';
import { CurrentUserService } from 'src/core/services/current-user.service';
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
    private currentUser: CurrentUserService,
  ) { }

  public user: User;
  public allUsers$: Observable<User[]>;

  ngOnInit(): void {
    
    this.currentUser.currentUser$.subscribe((user) => {
      this.user = user;
    });

    this.allUsers$ = this.userService.getAllUsers().pipe(
      map(resp => {
        const users = resp.body as User[];

        el.style.margin = '2px 0 0 0';
        el.style.borderBottom = '2px solid #66fcf1';

        return users.filter(u => u.id != this.user.id);
      })
    );

    const el = document.getElementById('users') as HTMLElement;
    el.style.color = '#45a29e';

  }

}
