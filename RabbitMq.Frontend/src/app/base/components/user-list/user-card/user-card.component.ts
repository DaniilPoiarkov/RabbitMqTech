import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/models/user';

@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.sass']
})
export class UserCardComponent implements OnInit {

  constructor() { }

  @Input() user: User;

  ngOnInit(): void {
    
  }

}
