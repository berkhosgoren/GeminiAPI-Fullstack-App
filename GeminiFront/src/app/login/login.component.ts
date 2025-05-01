import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent {
    username: string = '';
    password: string = '';
    errorMessage: string = '';
    loading: boolean = false;
    
    constructor(private authService: AuthService, private router: Router) {}

    login() {
        this.loading = true;
        this.errorMessage = '';

        this.authService.login(this.username, this.password).subscribe({
        next: (response: any) => {
        localStorage.setItem('jwt', response.token);
        localStorage.setItem('username', response.username);
        localStorage.setItem('userId', response.userId);
        this.router.navigate(['/chat']);
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        this.errorMessage = 'Invalid username or password.';
      }
    });
  }
}

