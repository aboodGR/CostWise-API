# CostWise Project Context

## 1. Project Purpose

CostWise is a personal finance application.

The current project is the ASP.NET Core backend API.

The goal is to build a complete backend first and then create a React frontend that connects to this API.

CostWise is also being used as the main ASP.NET Core learning project.

The project should stay understandable and practical instead of adding advanced architecture only for appearance.

---

## 2. Tech Stack

Current backend stack:

- C#
- ASP.NET Core Web API
- .NET
- Entity Framework Core
- SQL Server
- LINQ
- JWT Authentication
- ASP.NET Core PasswordHasher
- Git
- GitHub
- Postman

Development tools:

- Visual Studio 2022
- SQL Server Management Studio
- Postman

---

## 3. Main Architecture

Current request flow:

Controller
-> Service Interface
-> Service
-> ApplicationDbContext
-> SQL Server

Main project folders:

- Controllers
- Services
- Interfaces
- Models
- DTOs
- Results
- Data
- Configuration
- Migrations

The project intentionally does not use Repository Pattern because EF Core DbContext/DbSet already provide the needed behavior for this project.

The project also does not currently use:

- CQRS
- MediatR
- Clean Architecture
- Microservices

---

## 4. Main Entities

### User

Current User concept:

- Id
- Email
- PasswordHash

Email is unique.

Passwords are hashed using ASP.NET Core PasswordHasher.

---

### Category

Category belongs to a User.

Main fields:

- Id
- Name
- UserId
- User
- Expenses collection

Category names are unique per user.

The database uses a composite unique index based on:

- UserId
- Name

---

### Expense

Expense belongs to:

- one User
- one Category

Main fields:

- Id
- Title
- Amount
- CategoryId
- Category
- Date
- Description
- UserId
- User

Expense ownership is always checked using UserId.

An Expense must not be allowed to use another user's Category.

---

### Income

Income belongs to one User.

Main fields:

- Id
- Title
- Amount
- Date
- Description
- UserId
- User

Income queries are filtered by UserId.

---

### ReportSummary

ReportSummary is not a database entity.

It is a calculated response model.

It currently contains:

- TotalIncome
- TotalExpenses
- Balance

Report calculations are user-specific.

---

## 5. DTOs

The API uses DTOs to separate API input/output from EF Core entities.

### Expense DTOs

- CreateExpenseDto
- UpdateExpenseDto
- ExpenseResponseDto

Expense response data includes useful API fields such as:

- Id
- Title
- Amount
- CategoryId
- CategoryName
- Date
- Description

The full Category EF navigation object is not returned to the client.

---

### Income DTOs

- CreateIncomeDto
- UpdateIncomeDto
- IncomeResponse

---

### Category DTOs

- CreateCategoryDto
- UpdateCategoryDto
- CategoryResponseDto

Category response does not expose the Expense entity collection.

---

### Auth DTOs

- RegisterDto
- LoginDto
- LoginResponseDto

RegisterDto accepts:

- Email
- Password

LoginDto accepts:

- Email
- Password

LoginResponseDto returns:

- Token
- Email

PasswordHash is never sent by the client and is never returned to the client.

---

## 6. Validation

Request DTOs currently use Data Annotation validation.

Examples include:

- Required
- EmailAddress
- MinLength
- MaxLength
- Range

Current examples:

Expense Title:

- Required
- MaxLength 20

Expense Amount:

- decimal range starting at 0.01

Expense CategoryId:

- must be greater than 0

Descriptions:

- MaxLength 1000

Category Name:

- Required
- MaxLength 20

Register:

- valid required email
- password minimum length

ASP.NET Core `[ApiController]` handles invalid DTO model state automatically.

---

## 7. Authentication

Authentication has been implemented.

### Registration Flow

RegisterDto
-> AuthController
-> IAuthService
-> AuthService
-> check duplicate email
-> create User
-> hash password
-> save User

Duplicate emails are not allowed.

---

### Login Flow

LoginDto
-> AuthController
-> AuthService
-> find User by email
-> VerifyHashedPassword
-> generate JWT
-> return LoginResponseDto

Invalid login returns Unauthorized.

---

## 8. JWT

JWT authentication has been implemented.

JWT configuration currently uses:

- Key
- Issuer
- Audience
- ExpireMinutes

JWT contains claims including:

- UserId
- Email

JWT is signed using a symmetric key and HMAC SHA256.

ASP.NET Core JWT bearer authentication validates tokens.

`UseAuthentication()` runs before `UseAuthorization()`.

Protected controllers use:

`[Authorize]`

Register and Login remain public.

---

## 9. User Ownership

User ownership has been implemented across the main financial data.

### Expense

Expense GET-all filters by UserId.

Expense GET-by-id checks:

- Expense Id
- UserId

Expense Update checks:

- Expense Id
- UserId

Expense Delete checks:

- Expense Id
- UserId

Expense creation sets UserId from the JWT claim.

---

### Income

Income GET-all filters by UserId.

Income GET-by-id checks:

- Income Id
- UserId

Income Update checks ownership.

Income Delete checks ownership.

Income creation sets UserId from JWT.

---

### Category

Categories belong to users.

Category GET-all filters by UserId.

Category GET-by-id checks ownership.

Category Update checks ownership.

Category Delete checks ownership.

Category creation sets UserId from JWT.

Category duplicate checks are per-user.

---

### Reports

Reports receive the authenticated UserId.

Expense and Income queries are filtered before calculating totals.

A user only receives their own:

- TotalIncome
- TotalExpenses
- Balance

---

## 10. EF Core

Completed EF Core topics/features include:

- DbContext
- DbSet
- SQL Server
- Code First
- Migrations
- CRUD
- Foreign keys
- One-to-many relationships
- Navigation properties
- Include
- LINQ with EF Core
- Change tracking
- AsNoTracking
- Async EF Core
- Fluent API
- Unique indexes
- Composite indexes
- User ownership relationships

Services are Scoped because they depend on ApplicationDbContext.

Read-only entity queries use AsNoTracking where appropriate.

Tracked entities are used for normal update/delete operations.

---

## 11. Delete Behavior

Adding Category ownership originally caused a SQL Server multiple cascade path error.

The Category -> Expense relationship was configured with a non-cascading delete behavior to avoid multiple cascade paths.

Do not casually change this relationship.

Financial history should not disappear because a Category is deleted.

---

## 12. Known EF Warning / Cleanup Item

EF Core has warned that decimal store precision is not explicitly configured for:

- Expense.Amount
- Income.Amount

This should be handled during final backend cleanup, likely using explicit decimal precision.

Do not forget this item.

---

## 13. Result<T> Error Handling

The project has started moving away from ambiguous `null` service responses.

Folder:

`Results`

Generic class:

`Result<T>`

Current properties:

- Success
- Data
- ErrorCode
- ErrorMessage

The first service method being converted is:

`ExpenseService.UpdateExpense`

Current expected outcomes include:

### Expense missing

ErrorCode:

`ExpenseNotFound`

ErrorMessage:

`Expense Not Found`

### Category missing

ErrorCode:

`CategoryNotFound`

ErrorMessage:

`Category Not Found`

### Success

Success is true and Data contains the updated Expense.

The controller translates the result into HTTP responses.

Current mapping:

- ExpenseNotFound -> 404 Not Found
- CategoryNotFound -> 400 Bad Request
- Success -> 200 OK

The Result<T> conversion is currently part of the active error-handling work and is not yet necessarily applied to every service method.

Do not automatically convert the entire project without reviewing which methods actually benefit from Result<T>.

---

## 14. Configuration

The project uses `appsettings.json`.

Current configuration areas include:

### Database

DefaultConnection

### CostWiseSettings

- Currency
- MaxExpenseAmount

### JWT

- Key
- Issuer
- Audience
- ExpireMinutes

Production secrets should not eventually live directly in committed configuration.

---

## 15. Middleware

The project includes request timing middleware.

The project also uses standard authentication and authorization middleware.

Authentication must execute before authorization.

---

## 16. Phase Progress

### Phase 1 - ASP.NET Core Fundamentals

DONE.

Covered:

- ASP.NET Core fundamentals
- Minimal APIs
- Controllers
- Dependency Injection
- Middleware
- Configuration

---

### Phase 2 - EF Core and Database

DONE.

Covered:

- DbContext
- DbSet
- SQL Server
- Code First
- Migrations
- CRUD
- relationships
- foreign keys
- navigation properties
- Include
- LINQ with EF Core
- tracking
- AsNoTracking
- async EF Core
- Fluent API basics
- database integrity rules

Advanced repository work, deeper transaction work, and unnecessary EF theory were intentionally skipped.

---

### Phase 3 - Final Backend Phase

IN PROGRESS.

Completed:

- DTOs
- Request/response mapping
- Validation
- User entity
- Registration
- Password hashing
- Login
- JWT generation
- JWT validation
- Authentication
- Authorization
- User-owned Expenses
- User-owned Income
- User-owned Categories
- User-specific Reports

Currently working on:

- Error handling using Result<T>

Still planned:

- finish practical error handling
- logging
- final cleanup
- decimal precision cleanup
- architecture/code review
- deployment

Optional:

- tests

---

## 17. Current Exact Checkpoint

The current active task is error handling.

A generic Result<T> has been introduced.

`UpdateExpense` has been changed from an ambiguous nullable return toward:

`Result<Expense>`

The service can now distinguish:

- ExpenseNotFound
- CategoryNotFound
- successful update

The Expense Controller now translates those service results into correct HTTP status codes.

Category updates also use `Result<Category>` to distinguish a missing category (404)
from a duplicate name (400). Create/update responses use the existing response DTOs.

Do not jump ahead and rewrite every service automatically.

Finish and understand this flow first.

---

## 18. Next Planned Work

Immediate next work:

1. Verify the completed Result<T> Expense update flow through API testing.
2. Decide where else Result<T> provides real value.
3. Add application logging using ILogger.
4. Perform final backend cleanup.
5. Fix remaining EF/database warnings such as decimal precision.
6. Review security and user ownership.
7. Review status codes and API responses.
8. Build and test the project.
9. Prepare the backend for deployment.
10. Deploy CostWise API.

---

## 19. After Backend Completion

After Phase 3 is complete, development focus moves to React.

Planned frontend topics include:

- React components
- state
- forms
- routing
- API calls
- authentication
- JWT handling
- CostWise dashboard
- Expense management
- Income management
- Category management
- Financial summary UI

The React frontend will connect to the existing CostWise ASP.NET Core API.

The developer does not need to finish every advanced ASP.NET Core topic before starting React.

CostWise is intended to complete the main backend learning stage.

---

## 20. Project Goal

The final CostWise project should demonstrate that the developer understands:

- ASP.NET Core APIs
- Controllers
- Services and Interfaces
- Dependency Injection
- EF Core
- SQL Server
- DTOs
- Validation
- async database operations
- Authentication
- Password hashing
- JWT
- Authorization
- user ownership
- error handling
- logging
- API/database security
- deployment

The goal is not to make the architecture unnecessarily complicated.

The goal is to build something clean, understandable, secure, and explainable.
