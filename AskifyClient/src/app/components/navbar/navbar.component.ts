import {AfterViewInit, ChangeDetectorRef, Component, inject, OnInit} from '@angular/core';
import {Button} from 'primeng/button';
import {NgOptimizedImage} from '@angular/common';
import {AvatarModule} from 'primeng/avatar';
import {MenuModule} from 'primeng/menu';
import {InputTextModule} from 'primeng/inputtext';
import {MenuItem} from 'primeng/api';
import {AuthService} from '../../services/auth.service';
import {Router} from '@angular/router';

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
export class NavbarComponent implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly cdr = inject(ChangeDetectorRef);

  menuItems: MenuItem[] | undefined;

  ngOnInit() {
    this.updateMenuItems(this.authService.isLoggedIn());

    this.authService.getAuthState().subscribe(
      (isLoggedIn: boolean) => {
        this.updateMenuItems(isLoggedIn);
        this.cdr.detectChanges();
      }
    );
  }

  private updateMenuItems(isLoggedIn : boolean) {
    if (isLoggedIn) {
      this.menuItems = [
        {
          label: 'Moje konto',
          icon: 'pi pi-user',
          routerLink: ['/profile']
        },
        {
          separator: true
        },
        {
          label: 'Wyloguj się',
          icon: 'pi pi-sign-out',
          command: () => {
            this.authService.signOut();
            window.location.reload()
          }
        }
      ];
    } else {
      this.menuItems = [
        {
          label: 'Zaloguj się',
          icon: 'pi pi-sign-in',
          routerLink: ['/sign-in']
        },
        {
          label: 'Zarejestruj się',
          icon: 'pi pi-user-plus',
          routerLink: ['/sign-up']
        }
      ];
    }
  }
}
