import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserLogin } from 'src/models/user-login';
import { UserRegister } from 'src/models/user-register';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = '/api/auth';

  constructor(
    private http: HttpService,
  ) { }

  public login(userLogin: UserLogin): Observable<HttpResponse<string>> {
    return this.http.putFullRequest(userLogin, this.baseUrl + '/login');
  }

  public register(userRegister: UserRegister): Observable<HttpResponse<string>> {
    return this.http.postFullRequest(userRegister, this.baseUrl);
  }
}
