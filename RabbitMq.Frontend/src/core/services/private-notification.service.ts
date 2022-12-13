import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PrivateNotification } from 'src/models/notifications/private-notification';
import { User } from 'src/models/user';
import { CurrentUserService } from './current-user.service';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root'
})
export class PrivateNotificationService {

  baseUrl = '/api/privateNotification';

  user: User;

  constructor(
    private http: HttpService,
    private currentUser: CurrentUserService
  ) { 
    this.currentUser.currentUser$
      .subscribe((user) => this.user = user);
  }

  public getAllNotifications(): Observable<HttpResponse<PrivateNotification[]>> {
    return this.http.getFullRequest(this.baseUrl + '?userId=' + this.user.id);
  }

  public sendNotification(notification: PrivateNotification) : Observable<HttpResponse<void>> {
    return this.http.postFullRequest(notification, this.baseUrl);
  }

  public deleteNotification(notificationId: number): Observable<HttpResponse<void>> {
    return this.http.deleteFullRequest(this.baseUrl + '?id=' + notificationId);
  }

}
