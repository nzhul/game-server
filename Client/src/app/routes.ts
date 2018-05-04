import { HomeComponent } from './home/home.component';
import { Routes } from '@angular/router';
import { AuthGuard } from './_guards/auth.guard';
import { UserListComponent } from './users/user-list/user-list.component';
import { UserListResolver } from './_resolvers/user-list.resolver';
import { UserDetailComponent } from './users/user-detail/user-detail.component';
import { UserDetailResolver } from './_resolvers/user-detail.resolver';

export const appRoutes: Routes = [
    { path: 'home', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'users', component: UserListComponent, resolve: { users: UserListResolver } },
            { path: 'users/:id', component: UserDetailComponent, resolve: { user: UserDetailResolver } }
        ]
    },
    { path: '**', redirectTo: 'home', pathMatch: 'full' },
];
