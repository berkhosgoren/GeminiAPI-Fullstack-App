import { Component, AfterViewChecked, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

interface Message {
  user: string;
  text: string;
}

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements AfterViewChecked, OnInit {
  messages: Message[] = [];
  userInput: string = '';
  loading: boolean = false;

  username: string | null = localStorage.getItem('username');
  userId: string | null = localStorage.getItem('userId');

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.scrollToBottom();
  }

  sendMessage(): void {
    if (!this.userInput.trim()) return;

    const token = localStorage.getItem('jwt');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    // Add to message view
    this.messages.push({ user: this.username || 'You', text: this.userInput });

    // Store locally for /history component
    const previous = JSON.parse(localStorage.getItem('previousQuestions') || '[]');
    previous.push(this.userInput);
    localStorage.setItem('previousQuestions', JSON.stringify(previous));

    this.loading = true;

    const payload = {
      inputText: this.userInput,
      userId: this.userId
    };

    this.http.post<any>('https://localhost:7085/api/Content/generate', payload, { headers }).subscribe({
      next: (response) => {
        this.loading = false;
        this.messages.push({ user: 'Bot', text: response.answer || 'No response.' });
        this.scrollToBottom();
      },
      error: () => {
        this.loading = false;
        this.messages.push({ user: 'Bot', text: 'There was an error processing your request.' });
        this.scrollToBottom();
      }
    });

    this.userInput = '';
  }

  scrollToBottom(): void {
    const messagesContainer = document.querySelector('.messages');
    if (messagesContainer) {
      messagesContainer.scrollTop = messagesContainer.scrollHeight;
    }
  }

  ngAfterViewChecked(): void {
    this.scrollToBottom();
  }
}
