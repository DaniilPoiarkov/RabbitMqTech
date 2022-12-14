import { ErrorHandler, Injectable, Injector } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ToastrNotificationService } from '../services/toastr-notification.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class ErrorInterceptorService extends ErrorHandler {

  constructor(
    private injector: Injector
  ) { 
    super();
  }

  get toastr(): ToastrNotificationService {
    console.log('Get toastr');
    return this.injector.get(ToastrNotificationService);
  }

  get router(): Router {
    return this.injector.get(Router);
  }

  override handleError(error: unknown): void {
    super.handleError(error);

    if(!(error instanceof HttpErrorResponse)) {
      this.toastr.error('Unexpected client-side error', 'Error');
      return;
    }

    if(error.status === 0) {
      this.toastr.error('No connection', 'Error');
      return;
    }

    if(error.status >= 500) {
      this.toastr.error('Unexpected server-side error', 'Error');
    }

    if(error.status === 401) {
      this.toastr.error('Authorization expired.', 'Unauthorized');
      this.router.navigate(['/auth/login']);
      return;
    }

    if(error.status >= 400) {
      this.toastr.error(error.error.Error, 'Error');
      return;
    }
  }

}
