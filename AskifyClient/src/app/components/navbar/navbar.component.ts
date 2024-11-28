import { Component } from '@angular/core';
import {Button} from 'primeng/button';
import {NgOptimizedImage} from '@angular/common';
import {AvatarModule} from 'primeng/avatar';
import {MenuModule} from 'primeng/menu';
import {InputTextModule} from 'primeng/inputtext';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    Button,
    NgOptimizedImage,
    AvatarModule,
    MenuModule,
    InputTextModule
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {

}
