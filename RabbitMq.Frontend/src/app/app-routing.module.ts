import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthRoutes } from './auth/auth-routes';
import { AuthGuard } from './auth/guards/auth.guard';
import { BaseComponent } from './base/base.component';

const routes: Routes = [
  ...AuthRoutes,
  { 
    path: '', 
    component: BaseComponent, 
    canActivate: [AuthGuard], 
    canActivateChild: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
