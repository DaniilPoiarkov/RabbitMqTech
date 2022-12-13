import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CoreModule } from 'src/core/core.module';
import { SharedModule } from 'src/shared/shared.module';
import { BaseComponent } from './base/base.component';
import { AuthModule } from './auth/auth.module';
import { FooterComponent } from './base/footer/footer.component';
import { ToastrModule } from 'ngx-toastr';
import { ToastrConfig } from './toastr-config';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtInterceptorService } from 'src/core/interceptors/jwt-interceptor.service';

@NgModule({
  declarations: [
    AppComponent,
    BaseComponent,
    FooterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AuthModule,
    CoreModule,
    SharedModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(ToastrConfig)
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptorService, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
