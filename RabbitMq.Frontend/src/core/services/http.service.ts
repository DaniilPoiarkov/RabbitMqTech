import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  baseUrl = environment.webAPI;
  headers = new HttpHeaders();

  constructor(
    private http: HttpClient,
  ) { }

  public getFullRequest<T>(url: string, httpParams?: HttpParams): Observable<HttpResponse<T>> {
    return this.http.get<T>(this.baseUrl + url, { observe: 'response', headers: this.headers, params: httpParams })
  }

  public postFullRequest<T>(payload: object, url: string) : Observable<HttpResponse<T>> {
    return this.http.post<T>(this.baseUrl + url, payload, { observe: 'response', headers: this.headers });
  }

  public putFullRequest<T>(url: string, payload?: object) : Observable<HttpResponse<T>> {
    return this.http.put<T>(this.baseUrl + url, payload, { observe: 'response', headers: this.headers });
  }

  public deleteFullRequest<T>(url: string) : Observable<HttpResponse<T>> {
    return this.http.delete<T>(this.baseUrl + url, { observe: 'response', headers: this.headers });
  }
}
