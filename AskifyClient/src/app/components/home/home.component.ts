import { Component } from '@angular/core';
import {AccordionModule} from 'primeng/accordion';
import {NgForOf} from '@angular/common';

interface FaqItem {
  question: string;
  answer: string;
}


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    AccordionModule,
    NgForOf
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  faqItems: FaqItem[] = [
    {
      question: 'Kto jest autorem projektu?',
      answer: 'Projekt został stworzony przez Justynę Kurcz i Jakuba Piekielniaka pod nadzorem mgr inż. Dariusza Piwko.'
    },
    {
      question: 'W jakim celu powstał projekt?',
      answer: 'Projekt został stworzony jako praca zaliczeniowa z przedmiotu Technologie Aplikacji Webowych II na Akademii Tarnowskiej.'
    },
    {
      question: 'Jakie technologie zostały wykorzystane w projekcie?',
      answer: 'W projekcie wykorzystano nowoczesny stos technologiczny składający się z: Angular (frontend), .NET (backend), PrimeNG (biblioteka komponentów UI), Entity Framework Core (ORM), PostgreSQL (baza danych) oraz Docker.'
    },
    {
      question: 'Jakie są główne funkcjonalności aplikacji?',
      answer: 'Aplikacja oferuje szereg funkcjonalności, w tym: system uwierzytelniania i autoryzacji, responsywny interfejs użytkownika, integrację z REST API, mechanizmy walidacji formularzy.'
    },
    {
      question: 'Jak działa architektura aplikacji?',
      answer: 'Aplikacja została zbudowana w oparciu o architekturę warstwową. Frontend wykorzystuje Angular z komponentami PrimeNG. Backend oparty jest o .NET 8 z architekturą Clean Architecture, wykorzystując wzorzec CQRS'
    },
    {
      question: 'Jakie są plany rozwoju projektu?',
      answer: 'W przyszłości planujemy rozszerzyć funkcjonalność o dodanie wielojęzyczności, integrację z dodatkowymi serwisami zewnętrznymi oraz optymalizację wydajności aplikacji.'
    }
  ];
}
