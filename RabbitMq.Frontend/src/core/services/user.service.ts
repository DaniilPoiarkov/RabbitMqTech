import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from 'src/models/user';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = '/api/user';

  constructor(
    private http: HttpService
  ) { }

  public getCurrentUser(): Observable<HttpResponse<User>> {
    return this.http.getFullRequest(this.baseUrl + '/current');
  }

  public getUserById(id: number): Observable<HttpResponse<User>> {
    return this.http.getFullRequest(this.baseUrl + '?id=' + id);
  }

  public getUserByEmail(email: string): Observable<HttpResponse<User>> {
    return this.http.getFullRequest(this.baseUrl + '/email?email=' + email);
  }

  public getAllUsers(): Observable<HttpResponse<User[]>> {
    return this.http.getFullRequest(this.baseUrl + '/all');
  }

  public setConnectionId(id: string): Observable<HttpResponse<void>> {
    return this.http.putFullRequest(this.baseUrl + '?connectionId=' + id, undefined);
  }

  public updateAvatar(id: number, avatarUrl: string): Observable<HttpResponse<void>> {
    return this.http.putFullRequest(this.baseUrl + '/avatar?userId=' + id + '&avatarUrl=' + avatarUrl);
  }

  public removeAvatar(id: number): Observable<HttpResponse<void>> {
    return this.http.putFullRequest(this.baseUrl + '/avatar/remove?userId=' + id);
  }
  
}
