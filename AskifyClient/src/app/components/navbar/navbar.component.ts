import {ChangeDetectorRef, Component, inject, OnInit} from '@angular/core';
import {Button} from 'primeng/button';
import {NgForOf, NgOptimizedImage} from '@angular/common';
import {AvatarModule} from 'primeng/avatar';
import {MenuModule} from 'primeng/menu';
import {InputTextModule} from 'primeng/inputtext';
import {MenuItem} from 'primeng/api';
import {AuthService} from '../../services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    NgOptimizedImage,
    AvatarModule,
    MenuModule,
    InputTextModule,
    Button,
    NgForOf
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly cdr = inject(ChangeDetectorRef);

  userMenuItems: MenuItem[] | undefined;
  menuItems: MenuItem[] | undefined;

  ngOnInit() {
    this.updateUserMenuItems(this.authService.isLoggedIn());
    this.initializeMenuItem();

    this.authService.getAuthState().subscribe(
      (isLoggedIn: boolean) => {
        this.updateUserMenuItems(isLoggedIn);
        this.cdr.detectChanges();
      }
    );
  }

  private initializeMenuItem() {
    this.menuItems = [
      {
        label: 'Strona główna',
        icon: 'pi pi-home',
        routerLink: ['/strona-glowna']
      },
      {
        label: 'Pytania',
        icon: 'pi pi-question-circle',
        routerLink: ['/pytania']
      }
    ];

    if (this.authService.isAdmin()) {
      this.menuItems.push({
        label: 'Panel administracyjny',
        icon: 'pi pi-cog',
        routerLink: ['/panel-administracyjny']
      });
    }
  }

  private updateUserMenuItems(isLoggedIn : boolean) {
    if (isLoggedIn) {
      this.userMenuItems = [
        {
          label: 'Moje konto',
          icon: 'pi pi-user',
          routerLink: ['/moje-konto']
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
      this.userMenuItems = [
        {
          label: 'Zaloguj się',
          icon: 'pi pi-sign-in',
          routerLink: ['/zaloguj-sie']
        },
        {
          label: 'Zarejestruj się',
          icon: 'pi pi-user-plus',
          routerLink: ['/zarejestruj-sie']
        }
      ];
    }
  }
}
