import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { User } from '../_models/User';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { PaginatedResult } from '../_models/pagination';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable()
export class UserService {

    baseUrl = environment.apiUrl;

    constructor(private authHttp: HttpClient) { }

    getUsers(page?, itemsPerPage?, userParams?: any, likesParam?: string) {
        // use this when not using authHttp -> return this.authHttp.get(this.baseUrl + 'users', this.jwt())

        const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();
        let params = new HttpParams();

        if (page != null && itemsPerPage != null) {
            params = params.append('pageNumber', page);
            params = params.append('pageSize', itemsPerPage);
        }

        if (likesParam === 'Likers') {
            params = params.append('Likers', 'true');
        }

        if (likesParam === 'Likees') {
            params = params.append('Likees', 'true');
        }

        if (userParams != null) {
            params = params.append('minAge', userParams.minAge);
            params = params.append('maxAge', userParams.maxAge);
            params = params.append('gender', userParams.gender);
            params = params.append('orderBy', userParams.orderBy);
        }

        return this.authHttp.get<User[]>(this.baseUrl + 'users', { observe: 'response', params })
            .map(response => {
                paginatedResult.result = response.body;

                if (response.headers.get('Pagination') != null) {
                    paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
                }

                return paginatedResult;
            });
    }

    getUser(id): Observable<User> {
        return this.authHttp
            .get<User>(this.baseUrl + 'users/' + id);
    }

}
