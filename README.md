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

## Architektura i dependency injection

### Rola warstwy usług
Warstwa usług (`Services/`) odpowiada za realizację przypadków użycia aplikacji, koordynację operacji oraz egzekwowanie reguł biznesowych (np. zakaz cofania zamkniętego zadania ze statusu `Done` do `Todo` oraz walidacja daty wykonania `DueDate`). Oddziela ona protokół HTTP od logiki domenowej i dostępu do bazy danych. Dzięki temu endpointy w `Program.cs` są cienkie (Thin Endpoints) i odpowiadają wyłącznie za przyjęcie żądania oraz zwrócenie odpowiedniego kodu statusu HTTP (`200 OK`, `201 Created`, `204 NoContent`, `400 BadRequest`, `404 NotFound`).

### Główne interfejsy
- **`ITaskService`** — określa kontrakt dla operacji biznesowych na zadaniach (`GetAllAsync`, `GetByIdAsync`, `CreateAsync`, `ChangeStatusAsync`, `DeleteAsync`). Nie ujawnia szczegółów związanych z HTTP ani strukturą bazy danych.
- **`ITaskPriorityStrategy`** — określa kontrakt dla wymiennego algorytmu obliczającego priorytet zadania (`Calculate(TaskItem task)`).

### Lifetime usług
- **`ITaskService` (`TaskService`) -> `Scoped`**: Zarejestrowany jako `Scoped` (jedna instancja na żądanie HTTP), ponieważ wstrzykuje `AppDbContext`, który również posiada cykl życia `Scoped` i reprezentuje jednostkę pracy (Unit of Work) powiązaną z pojedynczym żądaniem. Rejestracja jako `Singleton` groziłaby problemami ze śledzeniem encji (trackingiem) i współbieżnością.
- **`ITaskPriorityStrategy` (`DueDatePriorityStrategy`) -> `Scoped`**: Lekka usługa bezstanowa wyliczająca priorytet na podstawie terminu realizacji zadania.

### Zastosowane wzorce i zasady SOLID
- **Service Pattern**: Przeniesienie przypadków użycia i reguł biznesowych z kontrolera/endpointów do `TaskService` (eliminacja code smell *Fat Controller*).
- **Strategy (`ITaskPriorityStrategy`, `DueDatePriorityStrategy`)**: Pozwala na wyliczanie priorytetu zadania według terminu wykonania bez rozbudowywania metody `CreateAsync` o kolejne instrukcje `if/else` (zgodność z **OCP**).
- **Single Responsibility Principle (SRP)**: Podział odpowiedzialności — endpointy obsługują wyłącznie HTTP, `TaskService` koordynuje procesy biznesowe zadań, a `DueDatePriorityStrategy` odpowiada wyłącznie za regułę kalkulacji priorytetu.
- **Dependency Inversion Principle (DIP) i Constructor Injection**: Endpointy zależą od abstrakcji `ITaskService`, a `TaskService` zależy od abstrakcji `ITaskPriorityStrategy`. Wszystkie zależności są jawnie przekazywane przez konstruktor, co ułatwia testowanie jednostkowe.

### Decyzja dotycząca Repository
W projekcie **świadomie zrezygnowano z tworzenia dodatkowej warstwy `Repositories/`**. Używany w projekcie Entity Framework Core (`AppDbContext` oraz `DbSet<TaskItem>`) sam w sobie stanowi abstrakcję nad dostępem do danych realizującą wzorce *Repository* oraz *Unit of Work*. Przy obecnej skali projektu i braku wielokrotnie powtarzających się, złożonych zapytań raportowych, dodatkowa klasa `TaskRepository` jedynie powielałaby metody `Add`, `FindAsync` i `SaveChangesAsync` z `DbSet`, zaciemniając prosty kod bez wniesienia wartości architektonicznej.
