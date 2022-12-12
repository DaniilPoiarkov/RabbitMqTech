import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class ToastrNotificationService {

  constructor(
    private toastr: ToastrService
  ) { }

  public success(
    message: string, 
    title?: string, 
    onTap?: () => void, 
    ms: number = 5000) : void {

    const toast = this.toastr.success(message, title, {
      timeOut: ms,
    });

    if(onTap) {
      toast.onTap.subscribe(() => onTap());
    }
  }

  public error(
    message: string, 
    title?: string,
    onTap?: () => void, 
    ms: number = 5000) : void {

    const toast = this.toastr.error(message, title, {
      timeOut: ms
    });

    if(onTap) {
      toast.onTap.subscribe(() => onTap());
    }
  }

  public info(
    message: string, 
    title?: string,
    onTap?: () => void, 
    ms: number = 5000) : void {

    const toast = this.toastr.info(message, title, {
      timeOut: ms
    });

    if(onTap) {
      toast.onTap.subscribe(() => onTap());
    }
  }

  public warning(
    message: string, 
    title?: string,
    onTap?: () => void, 
    ms: number = 5000) : void {

    const toast = this.toastr.warning(message, title, {
      timeOut: ms
    });

    if(onTap) {
      toast.onTap.subscribe(() => onTap());
    }
  }

}
