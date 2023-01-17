import { Component, OnInit, Input } from '@angular/core';
import { Observable } from 'rxjs';
import { CurrentUserService } from 'src/core/services/current-user.service';
import { User } from 'src/models/user';

@Component({
  selector: 'app-avatar',
  templateUrl: './avatar.component.html',
  styleUrls: ['./avatar.component.sass']
})
export class AvatarComponent implements OnInit {

  @Input() size = 45;
  @Input() isDisabled = false;

  style: string;
  
  constructor(
    private currentUser: CurrentUserService
  ) { }

  public user$: Observable<User>;

  ngOnInit(): void {
    this.user$ = this.currentUser.currentUser$;
    this.style = `width: ${this.size}px; height: ${this.size}px`;
  }

}
