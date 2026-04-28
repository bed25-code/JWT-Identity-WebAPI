# WebAPIAuth28April

`WebAPIAuth28April` is a .NET 8 ASP.NET Core Web API sample that demonstrates:

- user registration and login with ASP.NET Core Identity
- JWT bearer authentication
- role-based authorization
- Entity Framework Core with SQL Server
- Swagger/OpenAPI for local API exploration

The project includes a small set of demo endpoints that show anonymous, authenticated, and admin-only access patterns.

## Tech Stack

- .NET 8
- ASP.NET Core Web API
- ASP.NET Core Identity
- Entity Framework Core
- SQL Server / LocalDB
- JWT Bearer Authentication
- Swashbuckle Swagger

## Project Structure

```text
.
|-- WebAPIAuth28April.slnx
|-- README.md
`-- WebAPIAuth28April
    |-- Controllers
    |   |-- AuthController.cs
    |   `-- ProductsController.cs
    |-- DTOs
    |   `-- AuthDtos.cs
    |-- Data
    |   `-- AppDbContext.cs
    |-- Models
    |   |-- ApplicationUser.cs
    |   `-- JwtSettings.cs
    |-- Services
    |   |-- ITokenService.cs
    |   `-- TokenService.cs
    |-- Migrations
    |-- Properties
    |   `-- launchSettings.json
    |-- appsettings.json
    `-- WebAPIAuth28April.csproj
```

## Features

### Authentication

- `POST /api/auth/register` creates a new user
- `POST /api/auth/login` validates credentials and returns a JWT token

### Authorization

- authenticated endpoints use `[Authorize]`
- admin-only endpoints use `[Authorize(Roles = "Admin")]`
- public endpoints use `[AllowAnonymous]`

### Identity and Roles

- users are stored with ASP.NET Core Identity
- roles are created automatically at startup if missing:
  - `Admin`
  - `User`
- newly registered users are added to the `User` role

### JWT Claims

The generated token includes:

- user ID
- email
- full name
- role claims

## API Endpoints

### Auth

- `POST /api/auth/register`
- `POST /api/auth/login`

### Products

- `GET /api/products`  
  Requires authentication.

- `GET /api/products/public-info`  
  Public endpoint.

- `GET /api/products/my-profile`  
  Requires authentication and returns values from the current JWT claims.

- `GET /api/products/admin-only`  
  Requires the `Admin` role.

## Configuration

Main configuration lives in `WebAPIAuth28April/appsettings.json`.

### Connection String

The default configuration uses SQL Server LocalDB:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MyAppDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

If you are not using LocalDB, replace it with a connection string for your SQL Server instance.

### JWT Settings

```json
"JwtSettings": {
  "SecretKey": "your-super-secret-key-min-256-bits-long!!",
  "Issuer": "https://your-api.com",
  "Audience": "https://your-client.com",
  "ExpiryHours": 24
}
```

For local experiments, these values are enough to get started. For any real deployment, use a strong secret and environment-specific issuer/audience values.

## Prerequisites

Before running the API, make sure you have:

- .NET 8 SDK installed
- SQL Server LocalDB or another reachable SQL Server instance
- the `dotnet-ef` CLI tool installed if you want to apply migrations from the command line

Install EF Core CLI tools if needed:

```powershell
dotnet tool install --global dotnet-ef
```

## Getting Started

### 1. Restore dependencies

From the repository root:

```powershell
dotnet restore
```

### 2. Configure the database

Review `WebAPIAuth28April/appsettings.json` and update:

- `ConnectionStrings:DefaultConnection`
- `JwtSettings`

### 3. Apply database migrations

The project includes EF Core migrations, but it does not automatically run `Database.Migrate()` on startup. Apply the migrations before first use:

```powershell
dotnet ef database update --project .\WebAPIAuth28April\WebAPIAuth28April.csproj
```

### 4. Run the API

```powershell
dotnet run --project .\WebAPIAuth28April\WebAPIAuth28April.csproj
```

By default, the development launch settings use:

- `http://localhost:5211`
- `https://localhost:7231`

Swagger UI is available in development at:

- `https://localhost:7231/swagger`
- or `http://localhost:5211/swagger`

## Example Flow

### Register

```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Password1",
  "fullName": "Example User"
}
```

### Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Password1"
}
```

Example response:

```json
{
  "token": "<jwt-token>",
  "expiry": "2026-04-29T09:00:00Z"
}
```

### Call a protected endpoint

```http
GET /api/products
Authorization: Bearer <jwt-token>
```

## Notes and Caveats

- The `ProductsController` returns hard-coded demo data; products are not stored in the database.
- Roles are created automatically at startup, but no admin user is seeded by default.
- There is a custom authorization policy named `MinAge18` registered in the app, but it is not currently used by any endpoint.
- The sample request file `WebAPIAuth28April/WebAPIAuth28April.http` still references `/weatherforecast`, which is not part of the current API surface.
- Swagger is enabled only when the app runs in the `Development` environment.

## Development Notes

- `ApplicationUser` extends `IdentityUser` with a `FullName` field.
- `AppDbContext` inherits from `IdentityDbContext<ApplicationUser>`.
- JWT creation is handled by `TokenService`.

## Testing

There is currently no test project in the repository.

## Next Improvements

Useful next steps for this sample would be:

- add integration tests for auth and authorization flows
- seed an initial admin user for local development
- replace hard-coded product data with persisted entities
- move sensitive JWT and connection settings into environment variables or user secrets
