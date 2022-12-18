import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthRoutes } from './auth/auth-routes';
import { AuthGuard } from './auth/guards/auth.guard';
import { BaseComponent } from './base/base.component';
import { MainPageComponent } from './base/components/main-page/main-page.component';
import { PrivateNotificationListComponent } from './base/components/private-notification-list/private-notification-list.component';
import { SimpleNotificationListComponent } from './base/components/simple-notification-list/simple-notification-list.component';
import { UserListComponent } from './base/components/user-list/user-list.component';

const routes: Routes = [
  ...AuthRoutes,
  { 
    path: '',
    component: BaseComponent, 
    canActivate: [AuthGuard], 
    canActivateChild: [AuthGuard],
    children: 
    [
      { path: 'news', component: MainPageComponent },
      { path: 'users', component: UserListComponent },
      { path: 'private-notifications', component: PrivateNotificationListComponent },
      { path: 'simple-notifications', component: SimpleNotificationListComponent }
    ]
  },
  { path: '**', redirectTo: 'news' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
