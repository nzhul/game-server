import { HomeComponent } from './home/home.component';
import { Routes } from '@angular/router';
import { AuthGuard } from './_guards/auth.guard';

export const appRoutes: Routes = [
    { path: 'home', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        // children: [
        //     { path: 'members', component: MemberListComponent, resolve: { users: MemberListResolver } },
        //     { path: 'members/:id', component: MemberDetailComponent, resolve: { user: MemberDetailResolver } },
        //     {
        //         path: 'member/edit', component: MemberEditComponent,
        //         resolve: { user: MemberEditResolver }, canDeactivate: [PreventUnsavedChanges]
        //     },
        //     { path: 'messages', component: MessagesComponent, resolve: { messages: MessagesResolver } },
        //     { path: 'lists', component: ListsComponent, resolve: { users: ListsResolver } },
        // ]
    },
    { path: '**', redirectTo: 'home', pathMatch: 'full' },
];
