import { NgModule, ErrorHandler } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { ErrorInterceptorService } from './interceptors/error-interceptor.service';
import { SharedModule } from 'src/shared/shared.module';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    HttpClientModule,
    SharedModule
  ],
  providers: [
    { provide: ErrorHandler, useClass: ErrorInterceptorService },
  ]
})
export class CoreModule { }
