# Profiles API

ASP.NET Core service for clinic accounts and role profiles (admin, doctor, receptionist, patient). It stores profiles in PostgreSQL and creates the matching identity user through the Auth API (Keycloak).

Target framework: **.NET 10** (`net10.0`). Nullable reference types and implicit usings are enabled in every project.

## Solution layout

```
ProfilesApi.slnx
docker-compose.yml
ProfilesApi.Api/              HTTP host
ProfilesApi.Application/      use cases, DTOs, validation
ProfilesApi.Domain/           entities and persistence contracts
ProfilesApi.Infrastructure/   EF Core, PostgreSQL, repositories
```

Dependency direction:

```
Api ──► Application ──► Domain
 │
 └──► Infrastructure ──► Domain
```

Application does not reference Infrastructure. Infrastructure does not reference Application. The HTTP client that calls Auth is an Application interface (`IIdentityClient`) implemented in the API project (`AuthIdentityClient`).

## Projects

### ProfilesApi.Domain

Business model and persistence contracts. No reference to the other projects.

| Folder | Contents |
| --- | --- |
| `Entities` | `BaseEntity` (`Id`, audit fields), `SoftDeletableEntity`, `AccountEntity`, `PersonEntity`, and role entities `AdminEntity`, `DoctorEntity`, `PatientEntity`, `ReceptionistEntity` |
| `Enum` | `AccountRole`: Admin = 1, Receptionist = 2, Doctor = 3, Patient = 4 |
| `Interfaces` | `IGenericRepository<T>`, one repository interface per aggregate, `IUnitOfWork` |
| `Exceptions` | `AppException`, `ConflictException`, `InvalidFormatException`, `ServerException`, `IdentityProviderException` |

`AccountEntity` holds email, phone, name, role, photo id, and a local password hash. Each role entity extends `PersonEntity` (`AccountId`, `BirthDate`). Patient adds an insurance number. Doctor, admin, and receptionist add a work start date and office id. Doctor also has a specialization id.

`Account.Id` is the Keycloak user id returned by Auth.

### ProfilesApi.Application

Use cases and the types controllers exchange with clients. References Domain only.

| Folder | Contents |
| --- | --- |
| `Services` | `AccountService`, `AdminService`, `DoctorService`, `PatientService`, `ReceptionistService`, `RegistrationService` |
| `Interfaces` | Service interfaces and `IIdentityClient` |
| `DTOs` | Create, update, filter, and response records, plus polymorphic registration DTOs |
| `Validators` | FluentValidation validators, including registration rules |
| `Mappings` | Entity ↔ DTO extension methods |
| `Helpers` | `PaginationHelper` |
| `Security` | `PasswordHasher` (PBKDF2). The real password is sent to Auth; Profiles stores only this hash |

`DependencyInjection.AddApplicationServices` registers validators from this assembly, SharpGrip automatic MVC validation, and the scoped services.

Registration (`RegisterUserDto`) is one polymorphic body discriminated by `role`. `RegistrationService` sends email, password, first name, last name, and role to Auth, then saves the account and the role row in one transaction. If the database save fails, it asks Auth to delete the identity user.

### ProfilesApi.Infrastructure

PostgreSQL persistence. References Domain only.

| Folder | Contents |
| --- | --- |
| `Context` | `ProfilesDbContext` (Npgsql) |
| `Configurations` | EF configurations per entity |
| `Repositories` | `GenericRepository`, concrete repositories, `UnitOfWork` |
| `Interceptors` | `TimestampInterceptor`, `SoftDeleteInterceptor` |
| `Data` | `DatabaseOptions` bound from `ConnectionStrings` |
| `Migrations` | EF Core migrations |

`DependencyInjection.AddInfrastructure` requires `ConnectionStrings:ProfilesDb`, registers every `IInterceptor` in this assembly, a `ProfilesDbContext` factory, the unit of work, and the repositories.

### ProfilesApi.Api

The executable web project. References Application and Infrastructure.

| Folder | Contents |
| --- | --- |
| `Controllers` | `AccountController`, `AdminController`, `DoctorController`, `PatientController`, `ReceptionistController`, `RegistrationController`. Routes are `/{controller}` |
| `Authorization` | Named role policies in `Policies`. Fallback policy requires an authenticated user |
| `Clients` | `AuthIdentityClient` (`POST /Auth/register`, `DELETE /Auth/users/{id}`) |
| `Extensions` | Service registration, Keycloak JWT authentication, Swagger |
| `Middlewares` | `GlobalExceptionHandler` (`application/problem+json`) |
| `Options` | `AuthApiOptions`, `KeycloakOptions` |

Authentication is JWT Bearer against the Keycloak realm. The access token is read from the `accessToken` cookie when that cookie is present, otherwise from the `Authorization` header. Realm roles are copied onto `role` claims. Audience check accepts `aud` or `azp` equal to `Keycloak:Audience`.

Swagger UI is enabled in Development at `/swagger`. HTTPS redirection runs outside Development.

`GlobalExceptionHandler` maps argument and format errors and `ConflictException` to 400, `IdentityProviderException` to 502, `UnauthorizedAccessException` to 401, `KeyNotFoundException` to 404, and anything else to 500. 500 responses hide the exception message.

## NuGet packages

Versions are the ones declared in the project files.

| Project | Package | Version |
| --- | --- | --- |
| Domain | Microsoft.EntityFrameworkCore | 10.0.12 |
| Application | FluentValidation | 12.2.0 |
| Application | FluentValidation.DependencyInjectionExtensions | 12.2.0 |
| Application | Microsoft.EntityFrameworkCore | 10.0.14 |
| Application | SharpGrip.FluentValidation.AutoValidation.Mvc | 2.0.0 |
| Infrastructure | Microsoft.EntityFrameworkCore | 10.0.14 |
| Infrastructure | Microsoft.EntityFrameworkCore.Tools | 10.0.14 |
| Infrastructure | Microsoft.Extensions.Configuration.Abstractions | 10.0.14 |
| Infrastructure | Microsoft.Extensions.Configuration.UserSecrets | 10.0.14 |
| Infrastructure | Microsoft.Extensions.Options.ConfigurationExtensions | 10.0.14 |
| Infrastructure | Npgsql.EntityFrameworkCore.PostgreSQL | 10.0.3 |
| Api | Microsoft.AspNetCore.Authentication.JwtBearer | 10.0.14 |
| Api | Microsoft.AspNetCore.OpenApi | 10.0.12 |
| Api | Microsoft.EntityFrameworkCore.Design | 10.0.12 |
| Api | Microsoft.EntityFrameworkCore.Relational | 10.0.14 |
| Api | Swashbuckle.AspNetCore | 10.2.3 |

Domain references EF Core because `IUnitOfWork.BeginTransactionAsync` returns `IDbContextTransaction`.

## Configuration

`ProfilesApi.Api/appsettings.json` holds non-secret defaults:

| Key | Purpose |
| --- | --- |
| `AuthApi:BaseUrl` | Auth API base URL. Default `http://localhost:5119` |
| `Keycloak:BaseUrl` | Keycloak base URL. Default `http://localhost:8080` |
| `Keycloak:Realm` | Realm used as the JWT authority. Default `myrealm` |
| `Keycloak:Audience` | Expected token audience / authorized party. Default `dotnet-auth-service` |
| `Keycloak:RequireHttpsMetadata` | JWT metadata HTTPS requirement. Default `false` for local HTTP |

`ConnectionStrings:ProfilesDb` is required at startup and is not in `appsettings.json`. Set it with user secrets on the API project (the process loads that project's secrets):

```powershell
dotnet user-secrets set "ConnectionStrings:ProfilesDb" "Host=localhost;Port=5432;Database=profiles;Username=postgres;Password=postgres" --project ProfilesApi.Api
```

The API user-secrets id is `0f88a1bd-db5c-4c47-a64a-013e641a660b`.

## Setup and commands

From this directory (`ProfilesApi`):

```powershell
dotnet restore ProfilesApi.slnx
dotnet build ProfilesApi.slnx
dotnet run --project ProfilesApi.Api --launch-profile http
```

The `http` profile listens on `http://localhost:5082`. Swagger: `http://localhost:5082/swagger`. The `https` profile also listens on `https://localhost:7073`.

Apply migrations (startup project supplies configuration):

```powershell
dotnet ef database update --project ProfilesApi.Infrastructure --startup-project ProfilesApi.Api
```

Add a migration:

```powershell
dotnet ef migrations add <Name> --project ProfilesApi.Infrastructure --startup-project ProfilesApi.Api
```

`docker-compose.yml` starts SonarQube and its own Postgres 15, not the Profiles schema:

- Postgres: `localhost:5432`, user `sonar`, password `sonar`, database `sonar`
- SonarQube: `http://localhost:9000`

```powershell
docker compose up -d
```

Local Auth must be running for registration. Keycloak realm roles must match `AccountRole` names: `Admin`, `Receptionist`, `Doctor`, `Patient`.
