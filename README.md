# Askify - Serwis Społecznościowy do Zadawania Pytań

## Opis Projektu
Askify to aplikacja webowa umożliwiająca użytkownikom zadawanie pytań oraz odpowiadanie na nie w ramach społeczności. Projekt został wykonany jako część zaliczenia przedmiotu **Technologie Aplikacji Webowych II** na **Akademii Tarnowskiej**.

### Technologie użyte w projekcie
- **Backend**: C# + .NET
- **Frontend**: Angular
- **Baza danych**: PostgreSQL

---

## Możliwości systemu
- Rejestracja i logowanie użytkowników
- Dodawanie pytań przez użytkowników
- Odpowiadanie na pytania
- Przeglądanie pytań
- Profil użytkownika

---

## Struktura projektu
```
Askify/
├── AskifyAPI/               # Backend
│   ├── Api/                 # Warstwa API
│   ├── Application/         # Logika biznesowa
│   ├── Core/                # Modele domenowe
│   └── Infrastructure/      # Dostęp do danych, usługi zewnętrzne
└── AskifyClient/            # Frontend
    └── src/                 # Kod źródłowy aplikacji klienckiej
```

---

## Instrukcja uruchomienia
### Backend
1. Otwórz terminal i przejdź do katalogu:
   ```bash
    Askify/AskifyAPI/src/AskifyA.Api
   ```
2. Uruchom aplikację backendową za pomocą:
   ```bash
   dotnet run
   ```
3. Backend będzie dostępny pod adresem `http://localhost:5244` (domyślnie).

### Frontend
1. Otwórz nowy terminal i przejdź do katalogu:
   ```bash
    Askify/AskifyClient/src
   ```
2. Zainstaluj zależności używając:
   ```bash
   npm install
   ```
3. Uruchom aplikację kliencką za pomocą:
   ```bash
   ng serve
   ```
4. Frontend będzie dostępny pod adresem `http://localhost:4200` (domyślnie).

---

## Autorzy
Projekt został zrealizowany przez **Justynę Kurcz** oraz **Jakuba Piekielniaka** w ramach zaliczenia przedmiotu **Technologie Aplikacji Webowych II**.

Prowadzący: **mgr inż. Dariusz Piwko**

---
