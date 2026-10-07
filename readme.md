# 📱 SCD-Proiect: Social Media Platform

> A full-stack social media application with a **Spring Boot** REST API, a **Vue 3** web client and a **C# WinForms** desktop admin dashboard. Users register, log in, publish posts and comment. Admins moderate content from the web app or the desktop tool.

![Status](https://img.shields.io/badge/status-in%20development-yellow)
![Java](https://img.shields.io/badge/backend-Spring%20Boot-6DB33F)
![Vue](https://img.shields.io/badge/frontend-Vue%203-42B883)
![.NET](https://img.shields.io/badge/desktop-.NET%208%20WinForms-512BD4)
![License](https://img.shields.io/badge/license-MIT-blue)

## Table of Contents

1. [Overview](#overview)
2. [Features](#features)
3. [Architecture](#architecture)
4. [Diagrams](#diagrams)
5. [Repository Structure](#repository-structure)
6. [Tech Stack](#tech-stack)
7. [Getting Started](#getting-started)
8. [Configuration](#configuration)
9. [Roadmap](#roadmap)
10. [Security Notes](#security-notes)
11. [Contributing](#contributing)
12. [License](#license)

## Overview

SCD-Proiect is a university project that implements a small social network. The backend exposes a secured REST API. Two clients use it: a web application for regular users and a desktop dashboard for administrators.

## Features

**Users**
- Registration and login with JWT authentication
- Email service (for example account or notification emails)
- Role-based access (user and admin)

**Content**
- Create posts, with a status for each post (moderation workflow)
- Comment on posts
- Post feed on the home page

**Administration**
- Admin endpoints and an admin view in the web app
- Desktop admin dashboard (Windows) with its own login

## Architecture

```
                   ┌────────────────────────────┐
  Vue 3 web app ──►│                            │──► Database
  (users, admins)  │  Spring Boot REST API      │
  WinForms app  ──►│  JWT security, email       │──► SMTP server
  (admins)         └───────────────────────────┘
```

## Diagrams

| Diagram | File |
|---|---|
| Use-case diagram | ![Use-case diagram](use-case-diagram.png) |
| Sequence diagram for a process | ![Sequence diagram](sequence-diagram.png) |

## Repository Structure

```
SCD-Proiect/
├── Backend/
│   └── ProiectSCD/                 # Spring Boot (Maven)
│       └── src/main/java/com/utcn/ProiectSCD/
│           ├── admin/              # admin endpoints
│           ├── auth/               # login and registration
│           ├── comment/            # comments
│           ├── config/             # security, JWT, email, app config
│           ├── email/              # email service
│           ├── post/               # posts and post status
│           └── user/               # users and roles
├── FE/
│   └── socialmedia-frontend/       # Vue 3 + Vite + Pinia + Vuetify
│       └── src/
│           ├── views/              # Home, Login, Register, Admin
│           ├── components/         # CreatePost, PostCard
│           ├── stores/             # auth, posts (Pinia)
│           ├── services/api.js     # API client
│           └── router/
├── dekstopAdminApp/
│   └── AdminDashboard/             # C# WinForms (.NET 8)
│       └── AdminDashboard/
│           ├── Forms/              # LoginForm, DashboardForm
│           ├── Services/           # AdminService, AuthenticationService
│           └── Models/
├── use-case-diagram.png
├── sequence-diagram.png
└── readme.md
```

## Tech Stack

| Area | Technology |
|---|---|
| Backend | Java, Spring Boot, Spring Security, JWT, Maven |
| Database | <!-- TODO: MySQL / PostgreSQL / H2 --> |
| Web client | Vue 3, Vite, Pinia, Vue Router, Vuetify, ESLint, Prettier |
| Desktop client | C#, .NET 8, Windows Forms |
| Tools | IntelliJ IDEA, VS Code, Visual Studio, Git |

## Getting Started

### Prerequisites

- JDK <!-- TODO: version, e.g. 17 or 21 --> and Maven (or the included `mvnw`)
- Node.js (LTS) and npm
- .NET 8 SDK and Windows (for the desktop app)
- A running database and an SMTP account for emails

### 1. Clone

```bash
git clone https://github.com/ADobrovolschi/SCD-Proiect.git
cd SCD-Proiect
```

### 2. Backend

```bash
cd Backend/ProiectSCD
./mvnw spring-boot:run
```

### 3. Web frontend

```bash
cd FE/socialmedia-frontend
npm install
npm run dev
```

### 4. Desktop admin app (Windows)

Open `dekstopAdminApp/AdminDashboard/AdminDashboard.sln` in Visual Studio, set the API address in `Models/ApiConfig.cs` and run the project.

## Configuration

Secrets such as database passwords, the JWT signing key and SMTP credentials must not be committed. Use environment variables in `application.properties`:

```properties
spring.datasource.url=${DB_URL}
spring.datasource.username=${DB_USER}
spring.datasource.password=${DB_PASSWORD}
spring.mail.username=${MAIL_USER}
spring.mail.password=${MAIL_PASSWORD}
application.jwt.secret=${JWT_SECRET}
```

<!-- TODO: match these property names to the ones in application.properties and the config classes -->

The web client reads the API base URL from `src/services/api.js`.

## Roadmap

- [ ] Likes, shares and follow system
- [ ] Image upload for posts and profile pictures
- [ ] Search and hashtags
- [ ] Real-time notifications (WebSocket) and email digests
- [ ] Password reset and email verification
- [ ] Refresh tokens and rate limiting
- [ ] Moderation tools: reports, bans, audit log
- [ ] Statistics charts in the desktop dashboard
- [ ] Swagger/OpenAPI documentation
- [ ] Unit and integration tests, GitHub Actions CI
- [ ] Docker Compose for API, database and web client
- [ ] Deployment (cloud or VPS) and a mobile-friendly UI

## Security Notes

- Keep secrets out of Git and rotate any key that was ever committed. Deleting a file does not remove it from Git history.
- Never commit build output (`bin/`, `obj/`, `target/`, `node_modules/`) or IDE folders (`.vs/`, `.idea/`).
- Store passwords only as salted hashes (BCrypt).

## Contributing

1. Fork the repository
2. Create a branch: `git checkout -b feature/likes`
3. Commit: `git commit -m "Add likes"`
4. Push and open a pull request

## License

Distributed under the MIT License. See `LICENSE`.

## Author

**Alexandru Dobrovolschi** · [@ADobrovolschi](https://github.com/ADobrovolschi)
