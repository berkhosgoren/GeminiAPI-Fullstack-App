import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    constructor(private http: HttpClient) {}

    login(username: string, password: string) {
        return this.http.post('https://localhost:7085/api/Auth/login', { username, password }).pipe(
            tap((response: any) => {
                localStorage.setItem('jwt', response.token);
                localStorage.setItem('userId', response.userId); 
            })
        );
    }
}
