import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PrivateNotification } from 'src/models/notifications/private-notification';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root'
})
export class PrivateNotificationService {

  private baseUrl = '/api/privateNotification';

  constructor(
    private http: HttpService
  ) { }

  public getAllNotifications(userId: number): Observable<HttpResponse<PrivateNotification[]>> {
    return this.http.getFullRequest(this.baseUrl + '?userId=' + userId);
  }

  public sendNotification(notification: PrivateNotification) : Observable<HttpResponse<void>> {
    return this.http.postFullRequest(notification, this.baseUrl);
  }

  public deleteNotification(notificationId: number): Observable<HttpResponse<void>> {
    return this.http.deleteFullRequest(this.baseUrl + '?id=' + notificationId);
  }

}
