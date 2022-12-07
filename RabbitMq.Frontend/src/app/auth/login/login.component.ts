import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.sass']
})
export class LoginComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }
  public log(val: string){
    console.log(val);
  }
  public logEnter(val: string){
    console.log(val + ' after enter');
  }
  public clickedBtn(): void {
    console.log('clicked');
  }
}
