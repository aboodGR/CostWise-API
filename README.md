# CostWise API

CostWise is an ASP.NET Core Web API for personal finance management.

I made this project to practice the things I have been learning in .NET and ASP.NET Core and put them into one real project instead of only doing small examples.

The backend is almost finished. Later I plan to build a React frontend and connect it to this API.

I built this project while learning backend development with .NET, with a focus on keeping the code simple, secure, and understandable.

## Features
>>>>>>> bdfdd89 (Complete CostWise backend authentication security and API testing)

- User registration and login
- Password hashing
- JWT authentication
- Protected API endpoints
- User-owned expenses
- User-owned income
- User-owned categories
- Financial summaries
- CRUD operations
- DTOs for request and response models
- Data Annotation validation
- Service-layer error handling using `Result<T>`
- User ownership and access control
- Async Entity Framework Core operations
- Request timing and application logging
- SQL Server database with EF Core migrations


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

## Security

CostWise uses JWT authentication.

Each authenticated user can only access their own:

- Expenses
- Income
- Categories
- Financial reports

The UserId is taken from the authenticated JWT instead of being trusted from client input.

Expense category ownership is also validated to prevent one user from using another user's category.
>>>>>>> bdfdd89 (Complete CostWise backend authentication security and API testing)

## Technologies

- C#
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- LINQ
<<<<<<< HEAD
- JWT Authentication
- Git and GitHub

- JWT Bearer Authentication
- ASP.NET Core PasswordHasher
- Git
- GitHub
- Visual Studio
>>>>>>> bdfdd89 (Complete CostWise backend authentication security and API testing)
- Postman
- OpenAI Codex

## Architecture

The project follows a simple layered structure:

Controller  
→ Service Interface  
→ Service  
→ ApplicationDbContext  
→ SQL Server

Main folders:

- Controllers
- Services
- Interfaces
<<<<<<< HEAD
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

- Models
- DTOs
- Results
- Data
- Configuration
- Migrations

## AI-Assisted Development Workflow

I used OpenAI Codex during development as an AI coding and review assistant.

Instead of allowing unrestricted changes, I created repository-level Markdown instruction files:

- `AGENTS.md` — architecture, security, coding, and editing rules
- `PROJECT.md` — project context, implemented features, and development status
- `TESTING.md` — API and security testing workflow

Codex was used to:

- inspect the existing project
- review architecture and security
- identify bugs
- make controlled code changes
- review diffs
- build the project
- perform API and QA testing

All generated changes were reviewed and understood before being accepted.

## API Testing

The backend was manually tested across more than 100 checks covering:

- registration and login
- invalid login attempts
- JWT authorization
- category CRUD
- expense CRUD
- income CRUD
- DTO validation
- invalid category handling
- deleted and missing resources
- user ownership isolation
- cross-user access attempts
- cross-user category attacks
- financial report isolation

Two separate test users were used to verify that users cannot access or modify each other's financial data.
>>>>>>> bdfdd89 (Complete CostWise backend authentication security and API testing)

## Running the Project

1. Configure the SQL Server connection string.
2. Configure the JWT settings.
3. Apply EF Core migrations.
4. Run the project:


Run:
dotnet run

And feel free to test it on Postman

```bash
dotnet run
>>>>>>> bdfdd89 (Complete CostWise backend authentication security and API testing)
