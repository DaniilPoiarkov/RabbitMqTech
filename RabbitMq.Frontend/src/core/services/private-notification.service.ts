import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PrivateNotification } from 'src/models/notifications/private-notification';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root'
})
export class PrivateNotificationService {

  baseUrl = '/api/privateNotification';

  constructor(
    private http: HttpService
  ) { }

  public getAllNotifications(): Observable<HttpResponse<PrivateNotification[]>> {
    return this.http.getFullRequest(this.baseUrl + '?userId=' + 4); //TODO: implement currentUserService, inject and replace with correct Id 
  }

  public sendNotification(notification: PrivateNotification) : Observable<HttpResponse<void>> {
    return this.http.postFullRequest(notification, this.baseUrl);
  }

  public deleteNotification(notificationId: number): Observable<HttpResponse<void>> {
    return this.http.deleteFullRequest(this.baseUrl + '?id=' + notificationId);
  }

}
