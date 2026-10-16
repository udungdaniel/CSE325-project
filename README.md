# ThreadLine — Clothing Store Management App

ThreadLine is a web-based clothing store management application built with ASP.NET Core Blazor, C#, Entity Framework Core, and SQLite. It helps clothing businesses manage products, sales, inventory, and daily operations.

## Live Application

- **Hosting platform:** Render
- **GitHub repository:** https://github.com/udungdaniel/CSE325-project
- **Deployment branch:** `render-fix-sync`

## Features

- Administrator authentication
- Product management
- Sales recording and management
- Inventory tracking
- User and account management
- Responsive web interface

## Technologies Used

- C#
- .NET 10
- Blazor
- ASP.NET Core Identity
- Entity Framework Core
- SQLite
- Docker
- Render

## Administrator Login — Deployed Application

Use the following credentials to sign in to the deployed ThreadLine application.

- **Email:** `danielalfred783@gmail.com`
- **Password:** 123@Test

**Security note:** Do not commit the actual administrator password to a public GitHub repository. Store it in a private password manager or another secure location.

## Run Locally

### Prerequisites

- .NET 10 SDK
- Visual Studio or Visual Studio Code
- Git

### Installation

Clone the repository:

```bash
git clone https://github.com/udungdaniel/CSE325-project.git
cd CSE325-project
```

Restore dependencies:

```bash
dotnet restore ThreadLine.csproj
```

Run the application:

```bash
dotnet run --project ThreadLine.csproj
```

Open the local URL displayed in the terminal.

## Deployment

The application uses Docker for deployment on Render.

- **Dockerfile:** `./Dockerfile`
- **Deployment branch:** `render-fix-sync`

To deploy updates, push your changes to the configured branch and check the Render deployment logs.

## Author

Daniel Alfred
PoryoculusRodrigo Jorquera

## Project Purpose

ThreadLine was developed as a clothing store management project to demonstrate web application development using modern .NET technologies.
