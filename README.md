# Vivo — URL Shortener

Vivo is a modern URL shortener web application built on **Clean Architecture**, orchestrated with **.NET Aspire**, and
featuring a frontend user interface in **Angular**.

---

## 🚀 Table of Contents

- [About the Project](#-about-the-project)
- [Tech Stack](#-tech-stack)
- [Architecture](#-architecture)
- [MVP Features](#-mvp-features)
- [API Endpoints](#-api-endpoints)
- [Prerequisites](#-prerequisites)
- [Running the Application](#-running-the-application)
- [Database & Migrations](#-database--migrations)

---

## 📌 About the Project

The application provides:

- Converting long URLs into concise, unique codes (Base62).
- Fast HTTP 302 redirection from a short code to the original target URL.
- Tracking redirect/click counts for each generated link.
- Managing shortened links via a web user interface.

---

## 🛠 Tech Stack

### Backend & Orchestration

- **.NET 10** (C#)
- **.NET Aspire** — microservices and web app orchestration, diagnostic dashboard
- **ASP.NET Core Web API** — REST API controllers
- **Entity Framework Core** — data access layer (ORM)
- **Microsoft SQL Server** — relational database

### Frontend

- **Angular 22** (TypeScript, SCSS/CSS, RxJS)
- **Vite / Angular CLI** — bundler and development environment
- **Vitest & ESLint** — unit testing and linting

---

## 🏛 Architecture

The project is designed following **Clean Architecture** principles with unidirectional dependencies:

```
┌──────────────────────────────────────────────┐
│                  ApiService                  │
│       (Controllers, API Contracts, CORS)     │
└──────────────┬───────────────────────────────┘
               │
               ▼
┌──────────────────────────────────────────────┐
│                 Application                  │
│      (Use Cases, Services, DTOs, Interfaces) │
└───────┬──────────────────────────────┬───────┘
        │                              │
        ▼                              ▼
┌──────────────┐             ┌─────────────────┐
│    Domain    │ ◄───────────┤ Infrastructure  │
│(Entities,    │             │(EF Core, DbCtx, │
│Value Objects)│             │ Repo, Base62)   │
└──────────────┘             └─────────────────┘
```

---

## ✨ MVP Features

- [x] Unique link code generation in Base62 format (4 to 12 alphanumeric characters).
- [x] URL validation (valid format and `http`/`https` scheme required).
- [x] `302 Found` redirect at `/{code}` with click counter incrementation.
- [x] Link expiration check (`ExpiresAt`).
- [x] Database collision handling for generated codes.
- [x] Viewing and browsing generated links in the web panel.

---

## 📡 API Endpoints

| Method | Path                 | Description                                                     |
|--------|----------------------|-----------------------------------------------------------------|
| `POST` | `/api/shortened-url` | Creates a new shortened URL from the provided `OriginalUrl`.    |
| `GET`  | `/api/shortened-url` | Retrieves a list of all shortened URLs (development endpoint).  |
| `GET`  | `/{code}`            | Redirects (`302`) to the target URL or returns `404 Not Found`. |
| `GET`  | `/health`            | Health check for the API application status.                    |

### Example Link Creation Request:

```http
POST /api/shortened-url
Content-Type: application/json

{
  "originalUrl": "https://example.com/very/long/url"
}
```

Response:

```json
{
  "shortUrl": "https://localhost:7123/aB3xtda"
}
```

---

## 💻 Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js](https://nodejs.org/) (version 20.x or newer) and `npm`
- [Docker Desktop](https://www.docker.com/) / Podman (required for containers orchestrated by Aspire / SQL Server)
- SQL Server instance or LocalDB

---

## 🏃 Running the Application

### 1. Running with .NET Aspire (Recommended)

The simplest way to run the entire environment (API + Frontend + Telemetry) is starting the `Vivo.AppHost` project:

```bash
dotnet run --project src/Vivo.AppHost/Vivo.AppHost.csproj
```

Once started, the console will output the URL to the **Aspire Dashboard**, where you can monitor all services, logs, and
access both the frontend and API.

### 2. Running Individual Projects

#### Backend (API):

```bash
dotnet run --project src/Vivo.ApiService/Vivo.ApiService.csproj
```

#### Frontend (Angular):

```bash
cd src/Vivo.Web
npm install
npm start
```

The frontend will be available at `http://localhost:4200/`.

---

## 🗄 Database & Migrations

The project uses Entity Framework Core with Code-First migrations.

### Applying migrations to the database:

```bash
dotnet ef database update --project src/Vivo.Infrastructure --startup-project src/Vivo.ApiService
```

### Adding a new migration:

```bash
dotnet ef migrations add <MigrationName> --project src/Vivo.Infrastructure --startup-project src/Vivo.ApiService
```
