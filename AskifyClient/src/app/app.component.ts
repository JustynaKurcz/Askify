import { Component } from '@angular/core';
import {RouterModule, RouterOutlet} from '@angular/router';
import {NavbarComponent} from './components/navbar/navbar.component';
import {ToastModule} from 'primeng/toast';
import {MessageService} from 'primeng/api';
import {CommonModule} from '@angular/common';
import {FooterComponent} from './components/footer/footer.component';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  imports: [CommonModule, RouterOutlet, RouterModule, ToastModule, NavbarComponent, FooterComponent],
  providers: [MessageService],
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'AskifyClient';
}
