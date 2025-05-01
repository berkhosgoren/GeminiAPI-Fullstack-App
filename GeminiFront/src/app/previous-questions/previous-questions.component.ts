import { Component, OnInit} from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-previous-questions',
  templateUrl: './previous-questions.component.html',
  styleUrls: ['./previous-questions.component.css']
})
export class PreviousQuestionsComponent implements OnInit {
  previousQuestions: {
    text: string;
    generatedResponse: string;
    createdAt: string;
  }[] = [];

  constructor(private http: HttpClient){}
  
  ngOnInit(): void {
      this.fetchHistory();
  }

  fetchHistory(): void {
    const token = localStorage.getItem('jwt');
    const userId = localStorage.getItem('userId');

    if (!token || !userId) return;

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get<any[]>('https://localhost:7085/api/Content/history', { headers }).subscribe({
      next: (data) => {
        this.previousQuestions = data;
      },
      error: (err) => {
        console.error('Error loading history', err);
      }
    });
  }
}
