import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SimpleNotification } from 'src/models/notifications/simple-notification';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root'
})
export class SimpleNotificationService {

  private baseUrl = '/api/simpleNotification';

  constructor(
    private http: HttpService
  ) { }

  public getAllNotifications(userId: number): Observable<HttpResponse<SimpleNotification[]>> {
    return this.http.getFullRequest(this.baseUrl + '?userId=' + userId);
  }

  public sendNotification(notification: SimpleNotification) : Observable<HttpResponse<void>> {
    return this.http.postFullRequest(notification, this.baseUrl);
  }

  public deleteNotification(notificationId: number): Observable<HttpResponse<void>> {
    return this.http.deleteFullRequest(this.baseUrl + '?id=' + notificationId);
  }
}
