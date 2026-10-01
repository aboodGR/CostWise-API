# CostWise API

CostWise API is a personal finance backend project that I built using ASP.NET Core.

I made this project to practice the things I have been learning in .NET and ASP.NET Core and put them into one real project instead of only doing small examples.

The backend is almost finished. Later I plan to build a React frontend and connect it to this API.

## What It Does

The API currently supports:

- User registration and login
- JWT authentication
- Protected API endpoints
- Categories
- Expenses
- Income
- CRUD operations
- Expense and income summaries
- User-owned data
- DTOs for requests and responses
- Validation
- SQL Server database connection
- Configuration using the Options Pattern
- Request timing middleware

Each logged-in user can only access their own expenses, income, categories, and financial summary.

## Technologies

- C#
- ASP.NET Core
- .NET
- Entity Framework Core
- SQL Server
- LINQ
- JWT Authentication
- Git and GitHub
- Postman

## Project Structure

The project uses:

- Controllers
- Services
- Interfaces
- Dependency Injection
- Entity Framework Core
- DTOs
- Middleware

The goal is to keep the project simple and clean while still using a proper backend structure.

## Database

The project uses Entity Framework Core with SQL Server.

It currently includes:

- Code First migrations
- Primary and foreign keys
- User relationships
- Category to Expense relationship
- Navigation properties
- Include
- AsNoTracking
- Async EF Core operations
- User-specific database queries

Categories are also owned by users, so different users can have their own categories with the same name.

## Authentication

CostWise now supports:

- User registration
- Login
- Password hashing
- JWT token generation
- JWT validation
- Protected endpoints using [Authorize]

The API stores the logged-in user's ID inside the JWT token and uses it to make sure users can only access their own data.

## DTOs and Validation

The API uses separate DTOs for creating, updating, and returning data.


Validation is also used for things like required fields, email format, amount ranges, and maximum lengths.

## Running the Project

Clone the repository and open the project.

Run:
dotnet run

And feel free to test it on Postman
