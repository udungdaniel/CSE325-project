# ThreadLine

A .NET Blazor web app that helps a small clothing store manage its sales and inventory in one place. Cashiers ring up sales, every sale automatically updates the stock, and the owner manages products and keeps an eye on what needs restocking.

Built as the group project for CSE 325 (.NET Development).

## Live Demo

- **Site:** `https://cse325-project-xoj0.onrender.com`
- **Repository:** https://github.com/udungdaniel/CSE325-project

## Demo Login

Use this account to try the app as an administrator:

| Role  | Email                    | Password                  |
|-------|--------------------------|---------------------------|
| Admin | `admin@threadline.local` | `Admin123!` |

This is a throwaway account created only for this class project. The demo site uses a temporary database, so data may reset when the site is redeployed.

## Features

- **Authentication with roles.** Users log in with ASP.NET Core Identity. Admins have full access, and Employees have a limited view.
- **Product management (CRUD).** Admins can add, edit, search, filter and delete products (name, category, size, color, price, stock and reorder level).
- **Stock status.** Each product shows whether it is in stock, low on stock or out of stock, based on its reorder level.
- **Sales and checkout.** Add products to a sale, see the total with tax, and complete the purchase. The sale is saved to the database.
- **Automatic inventory updates.** Completing a sale reduces the stock of every product sold.
- **Automated tests.** Unit tests cover the core business logic, such as tax calculation and inventory updates.

### Planned

- Dashboard with sales summaries for yesterday, this week and this month
- Restock estimates based on sales history

## Tech Stack

- .NET 10 and Blazor (Interactive Server rendering)
- ASP.NET Core Identity for authentication and roles
- Entity Framework Core with SQLite
- xUnit for tests
- Docker, deployed on Render

## Run It Locally

You need the [.NET 10 SDK](https://dotnet.microsoft.com/download).

1. Clone the repository:
   ```
   git clone https://github.com/udungdaniel/CSE325-project.git
   cd CSE325-project
   ```

2. Set the credentials for the seeded admin account, then start the app.

   macOS or Linux (bash/zsh):
   ```
   THREADLINE_ADMIN_EMAIL="admin@threadline.local" THREADLINE_ADMIN_PASSWORD="Your-Password1!" dotnet run
   ```

   Windows (PowerShell):
   ```
   $env:THREADLINE_ADMIN_EMAIL="admin@threadline.local"
   $env:THREADLINE_ADMIN_PASSWORD="Your-Password1!"
   dotnet run
   ```

3. Open the address shown in the terminal (usually `https://localhost:5001` or similar) and log in with the email and password you set.

The database is created automatically on the first run, so there is no separate migration step. The password must follow Identity's rules: at least 6 characters, with an uppercase letter, a lowercase letter, a digit and a symbol.

If the environment variables are not set, the app still starts, but no admin account is created.

## Run the Tests

```
dotnet test
```

## Roles

| Role     | What they can do |
|----------|------------------|
| Admin    | Manage products (add, edit, delete), process sales, view stock |
| Employee | Process sales and view dashboard |

New accounts created through the Register page are given the Employee role. The admin account is created by the seeder when the app starts.

## Project Structure

```
Components/   Blazor pages, layout and the Identity account pages
Data/         Database context, user model, migrations and the identity seeder
Models/       Product, Sale and related entities
Services/     Business logic (tax calculation, inventory updates)
ThreadLine.Tests/   Automated tests
```

## Team

- `<Name 1>`
- `<Name 2>`
