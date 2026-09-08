# Bookify

Bookify — сервис бронирования апартаментов на .NET, построенный по принципам
Clean Architecture и CQRS (MediatR).

## Архитектура

Решение разделено на четыре проекта:

- **Bookify.Domain** — доменные сущности (`Booking`, `Apartment`, `User`, `Review`),
  доменные события, паттерн `Result`/`Error`.
- **Bookify.Application** — команды и запросы (`ReserveBookingCommand`,
  `SearchApartmentsQuery`, `GetBookingQuery`, `RegisterUserCommand`,
  `LogInUserCommand`), абстракции (`IUnitOfWork`, `IBookingRepository`,
  `IDateTimeProvider`, `IEmailService`, `IJwtService`) и MediatR pipeline
  behaviors (логирование, валидация).
- **Bookify.Infrastructure** — EF Core (`ApplicationDbContext`, миграции,
  репозитории) и интеграция с Keycloak для аутентификации/JWT.
- **Bookify.Api** — контроллеры (`ApartmentsController`, `BookingsController`,
  `UsersController`), middleware обработки исключений.

## Стек

- ASP.NET Core
- EF Core + PostgreSQL
- MediatR (CQRS)
- Keycloak (аутентификация, JWT)
- Docker Compose

## Запуск

Сервисы (API, PostgreSQL, Keycloak) поднимаются через Docker Compose:

```bash
docker-compose up
```

Требуемые переменные окружения (`.env`):

```
DB_NAME=...
DB_USER=...
DB_PASSWORD=...
KEYCLOAK_ADMIN=...
KEYCLOAK_ADMIN_PASSWORD=...
```

## Миграции базы данных

Новая миграция создаётся и применяется скриптом `migrate.sh`:

```bash
./migrate.sh <MigrationName>
```
