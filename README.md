# PZL-Sledzinski-Patryk

## Opis
Repozytorium demonstracyjne przygotowane w ramach pierwszego zadania laboratoryjnego. Celem projektu jest potwierdzenie praktycznej znajomości podstawowego workflow systemu kontroli wersji Git, w tym pracy z gałęziami (branches), prowadzenia czytelnej historii zmian oraz realizacji procesu Pull Request i Code Review.

## Technologie
* C# 12
* .NET 9.0
* ASP.NET Core
* Git & GitHub

## Autor
* Patryk Śledziński (Indeks: 126084)

## Status
Projekt znajduje się obecnie na etapie przygotowania środowiska.

## Uruchomienie i przygotowanie środowiska
Do pobrania i uruchomienia projektu wymagane są:
1. Git
2. .NET 9.0 SDK

Instrukcja uruchomienia:
1. Sklonuj repozytorium: `git clone https://github.com/pzx126084/PZL-Sledzinski-Patryk.git`
2. Zbuduj projekt: `dotnet build src/DemoApp/DemoApp.csproj`
3. Uruchom aplikację: `dotnet run --project src/DemoApp/DemoApp.csproj`

## Baza danych

### Używana technologia
- **ORM:** Entity Framework Core (.NET 8)
- **Provider:** SQLite (Microsoft.EntityFrameworkCore.Sqlite)

### Sposób konfiguracji
Connection string przechowywany jest poza repozytorium przy użyciu narzędzia Secret Manager (User Secrets). Aby skonfigurować go lokalnie, przejdź do folderu src/DemoApp i wykonaj:
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Data Source=app.db"

### Sposób wykonania migracji
dotnet ef migrations add InitialCreate --project src/DemoApp

### Sposób utworzenia bazy
dotnet ef database update --project src/DemoApp

### Podstawowe encje
- **User:** Encja reprezentująca użytkownika (Id, Name). Relacja 1:N z zadaniami.
- **Project:** Encja reprezentująca projekt (Id, Title). Relacja 1:N z zadaniami.
- **TaskItem:** Encja reprezentująca zadanie (Id, Description, IsCompleted, ProjectId, UserId).

### Ograniczenia
- Na polach tekstowych zastosowano atrybuty [Required] oraz [MaxLength].
- Zastosowano więzy integralności kluczy obcych (ProjectId, UserId) łączące zadania z projektami i użytkownikami.
