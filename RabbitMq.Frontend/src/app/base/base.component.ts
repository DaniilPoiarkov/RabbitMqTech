import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrNotificationService } from 'src/core/services/toastr-notification.service';

@Component({
  selector: 'app-base',
  templateUrl: './base.component.html',
  styleUrls: ['./base.component.sass']
})
export class BaseComponent implements OnInit {

  constructor(
    private router: Router,
    private toastr: ToastrNotificationService,
  ) { }

  ngOnInit(): void {
    this.toastr.success('QWERTY', 'ASD');
    this.toastr.error('ERROR', 'ERROR');
  }

  logout(): void {
    //TODO Add toastr notifications
    localStorage.clear();
    this.router.navigate(['auth/login']);
  }
}
