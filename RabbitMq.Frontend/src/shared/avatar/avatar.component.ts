import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { User } from 'src/models/user';

@Component({
  selector: 'app-avatar',
  templateUrl: './avatar.component.html',
  styleUrls: ['./avatar.component.sass']
})
export class AvatarComponent implements OnInit {

  @Input() size = 45;
  @Input() isDisabled = false;
  @Input() user: User;

  @Output() clicked = new EventEmitter<void>();

  style: string;
  
  constructor(
  ) { }

  ngOnInit(): void {
    this.style = `width: ${this.size}px; height: ${this.size}px`;
  }

  click(): void {
    this.clicked.emit();
  }

}
