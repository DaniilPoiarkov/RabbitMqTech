import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AccessToken } from 'src/models/access-token';
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

  public login(userLogin: UserLogin): Observable<HttpResponse<AccessToken>> {
    return this.http.putFullRequest(userLogin, this.baseUrl);
  }

  public register(userRegister: UserRegister): Observable<HttpResponse<AccessToken>> {
    return this.http.postFullRequest(userRegister, this.baseUrl);
  }
}
