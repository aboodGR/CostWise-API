# CostWise API

CostWise API is a personal finance backend project that I built using ASP.NET Core.

I started this project mainly to practice the things I have been learning in .NET and ASP.NET Core and to put them into one real project instead of just doing small examples.

The project currently focuses on backend development and API design. Later I plan to build a React frontend and connect it to this API.

## What It Does

Right now the API supports:

- Categories
- Expenses
- Income
- CRUD operations
- Expense and income summaries
- Category and expense relationships
- Configuration using the Options Pattern
- Request timing middleware
- SQL Server database connection
- DTOs for API requests and responses

## Technologies

- C#
- ASP.NET Core
- .NET
- Entity Framework Core
- SQL Server
- LINQ
- Git and GitHub
- Postman

## Project Structure

The project uses:

- Controllers
- Services
- Interfaces
- Dependency Injection
- Entity Framework Core
- SQL Server
- DTOs
- Middleware
- Configuration through `appsettings.json`

The goal is to keep the project simple and clean while still following a proper backend structure.

## Database

The project uses Entity Framework Core with SQL Server.

Current database features include:

- Code First
- Migrations
- Primary and foreign keys
- Category to Expense relationship
- Navigation properties
- `Include` for related data
- `AsNoTracking` for read-only queries
- Async EF Core operations
- Foreign key validation before adding or updating expenses

## DTOs

DTOs are used so the API does not expose the database entities directly.

I currently use separate DTOs for creating, updating, and returning data.

### Expense

- `CreateExpenseDto`
- `UpdateExpenseDto`
- `ExpenseResponseDto`

### Income

- `CreateIncomeDto`
- `UpdateIncomeDto`
- `IncomeResponseDto`

### Category

- `CreateCategoryDto`
- `UpdateCategoryDto`
- `CategoryResponseDto`

This gives the API more control over what the client is allowed to send and what data gets returned.

## Running the Project

Clone the repository and open the project.

Then run:

dotnet run


The API can be tested using Postman .

## Project Status

Completed so far:

- ASP.NET Core Web API structure
- Controllers
- Services and Interfaces
- Dependency Injection
- Middleware
- Configuration with `appsettings.json`
- Options Pattern
- Entity Framework Core with SQL Server
- Code First migrations
- Category, Expense, and Income CRUD
- Category to Expense relationship
- Foreign key validation
- Related data loading using `Include`
- Read-only queries using `AsNoTracking`
- Async EF Core database operations
- Async service and controller methods
- Request DTOs
- Response DTOs
- DTO mapping between API data and database entities

## Next Steps

The next part of the project is focused on improving the API and making it closer to a real application.

Planned next steps:

- Validation
- Authentication
- Signup and Login
- Authorization
- User-owned expenses and income
- Better error handling
- Logging
- Final backend cleanup
- Deployment

After the backend is finished, I plan to build the CostWise frontend using React and connect it to this API.
