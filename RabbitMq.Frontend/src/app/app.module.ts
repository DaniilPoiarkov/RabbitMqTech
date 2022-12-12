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
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
