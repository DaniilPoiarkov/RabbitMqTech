import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SimpleNotification } from 'src/models/notifications/simple-notification';
import { User } from 'src/models/user';
import { CurrentUserService } from './current-user.service';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root'
})
export class SimpleNotificationService {

  baseUrl = '/api/simpleNotification';
  user: User;

  constructor(
    private http: HttpService,
    private currentUser: CurrentUserService
  ) { 
    this.currentUser.currentUser$
      .subscribe((user) => this.user = user);
  }

  public getAllNotifications(): Observable<HttpResponse<SimpleNotification[]>> {
    return this.http.getFullRequest(this.baseUrl + '?userId=' + this.user.id);
  }

  public sendNotification(notification: SimpleNotification) : Observable<HttpResponse<void>> {
    return this.http.postFullRequest(notification, this.baseUrl);
  }

  public deleteNotification(notificationId: number): Observable<HttpResponse<void>> {
    return this.http.deleteFullRequest(this.baseUrl + '?id=' + notificationId);
  }
}
