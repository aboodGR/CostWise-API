# CostWise Agent Instructions

## 1. Project Overview

CostWise is an ASP.NET Core Web API for personal finance management.

The project is being built as a learning project, but the code should still follow clean and realistic backend practices.

The application currently supports:

- User registration
- User login
- Password hashing
- JWT authentication
- Authorization
- User-owned Categories
- User-owned Expenses
- User-owned Income
- User-specific financial summaries
- DTOs
- Validation
- Entity Framework Core
- SQL Server
- Async database operations
- Service and interface layers
- Basic Result<T> based service error handling

Read `PROJECT.md` before making significant changes.

---

## 2. Architecture

The project currently follows a simple layered structure:

Controller
-> Interface
-> Service
-> ApplicationDbContext
-> SQL Server

Main folders:

- Controllers
- Services
- Interfaces
- Models
- DTOs
- Data
- Configuration
- Results
- Migrations

Do not introduce a different architecture unless explicitly requested.

---

## 3. Architecture Rules

Controllers must remain thin.

Controllers should handle:

- HTTP requests
- Reading authenticated user claims
- Mapping request DTOs when appropriate
- Calling services
- Translating service results into HTTP responses

Controllers should NOT contain:

- EF Core queries
- Direct ApplicationDbContext usage
- Large amounts of business logic
- Database rules

Business logic belongs in Services.

Database access belongs in Services through `ApplicationDbContext`.

Reuse existing Services and Interfaces before creating new abstractions.

Do not automatically add:

- Repository Pattern
- Unit of Work wrappers
- CQRS
- MediatR
- Clean Architecture
- Microservices
- unnecessary abstraction layers

`DbContext` and `DbSet` are already used directly inside the service layer.

Prefer the smallest clean change that fits the existing project.

---

## 4. DTO Rules

API request and response models should use DTOs where appropriate.

Current DTO groups include:

- Expense
- Income
- Category
- Auth

Examples:

- CreateExpenseDto
- UpdateExpenseDto
- ExpenseResponseDto
- CreateIncomeDto
- UpdateIncomeDto
- IncomeResponseDto
- CreateCategoryDto
- UpdateCategoryDto
- CategoryResponseDto
- RegisterDto
- LoginDto
- LoginResponseDto

Do not expose EF navigation properties to the client without a clear reason.

Do not allow clients to control backend-owned properties such as:

- UserId
- PasswordHash
- database-generated Id values

UserId must come from the authenticated JWT claim.

---

## 5. Authentication and Security Rules

CostWise uses JWT authentication.

Public endpoints:

- Register
- Login

Business/data endpoints are protected with `[Authorize]`.

JWT currently contains:

- UserId
- Email

Never put sensitive information such as passwords or password hashes inside JWT claims.

Passwords must never be stored as plain text.

Password hashing and verification use ASP.NET Core `IPasswordHasher<User>`.

Never trust a UserId supplied by the client.

UserId must come from the authenticated token.

---

## 6. User Ownership Rules

The following data is user-owned:

- Expenses
- Income
- Categories
- Financial summary/report data

A logged-in user must never be able to:

- read another user's Expense
- update another user's Expense
- delete another user's Expense
- read another user's Income
- update another user's Income
- delete another user's Income
- access another user's Categories
- modify another user's Categories
- receive another user's financial totals

Queries involving owned data must filter by UserId.

Examples of expected ownership checks:

`x.Id == id && x.UserId == userId`

or:

`Where(x => x.UserId == userId)`

Security ownership checks must not be removed for convenience.

---

## 7. Category Rules

Categories belong to users.

Category names are unique per user, not globally.

The unique database rule is conceptually:

`UserId + Name`

This means:

- User 1 can have "Food"
- User 2 can also have "Food"
- User 1 cannot create two "Food" categories

When checking duplicate categories, always include UserId.

When updating a category name, exclude the category currently being updated from duplicate-name checks.

---

## 8. Expense Rules

Expenses belong to users.

An Expense contains a CategoryId.

When creating or updating an Expense:

- verify that the Category exists
- verify that the Category belongs to the same authenticated user
- do not allow an Expense to reference another user's Category

Expense queries must be filtered by UserId.

---

## 9. Income Rules

Income belongs to users.

All Income read/update/delete operations must filter by UserId.

The client does not provide UserId.

UserId comes from the authenticated JWT claim.

---

## 10. Reports

Financial reports are user-specific.

Summary calculations must only use:

- Expenses belonging to the current user
- Income belonging to the current user

Never calculate totals across all users.

Current report output includes:

- TotalIncome
- TotalExpenses
- Balance

---

## 11. EF Core Rules

Use async EF Core methods for database I/O.

Examples:

- ToListAsync
- FirstOrDefaultAsync
- AnyAsync
- SumAsync
- SaveChangesAsync
- AddAsync

Do not use `.Result` or `.Wait()`.

Use `AsNoTracking()` for read-only entity queries.

Do not use `AsNoTracking()` when loading an entity that will be modified through normal EF change tracking.

Use `Include()` only when related data is actually required.

Do not add unnecessary database queries only for style.

Do not change the database schema unless the requested feature requires it.

If a schema change is required:

1. Explain why.
2. Identify the affected models/relationships.
3. Review the migration before applying it.

Do not delete or rewrite migrations without a clear reason.

---

## 12. Database Delete Behavior

The Category -> Expense relationship uses a delete behavior that avoids SQL Server multiple cascade path problems.

Do not change cascade behavior without reviewing all relationships first.

Deleting a Category must not unexpectedly delete financial history.

---

## 13. Validation

Request DTOs use Data Annotation validation where appropriate.

Examples:

- Required
- EmailAddress
- MinLength
- MaxLength
- Range

Do not duplicate simple DTO validation manually inside controllers.

`[ApiController]` already handles invalid model state automatically.

Business rules such as:

- Expense does not exist
- Category does not exist
- duplicate category
- ownership failure

are not the same as Data Annotation validation.

Handle those in the service/business layer.

---

## 14. Error Handling

The project is currently introducing a generic:

`Result<T>`

It contains:

- Success
- Data
- ErrorCode
- ErrorMessage

The purpose is to avoid returning `null` when one service operation can fail for multiple reasons.

Example:

UpdateExpense may return:

- Success
- ExpenseNotFound
- CategoryNotFound

Services describe what happened.

Controllers translate the result into HTTP responses.

Example mapping:

- ExpenseNotFound -> 404 Not Found
- CategoryNotFound -> 400 Bad Request
- Success -> 200 OK

Do not throw exceptions for normal business outcomes such as:

- record not found
- invalid category
- duplicate category

Exceptions should be reserved for unexpected failures.

Do not create a separate custom result class for every service method unless there is a strong reason.

Prefer the reusable `Result<T>` pattern.

---

## 15. HTTP Rules

Use appropriate HTTP responses.

Typical mappings:

- successful GET -> 200 OK
- successful update -> 200 OK
- invalid request/business input -> 400 Bad Request
- unauthenticated request -> 401 Unauthorized
- resource not found -> 404 Not Found

Do not return `200 OK` with null for missing resources.

Do not change status-code behavior without explaining why.

---

## 16. Configuration and Secrets

Configuration is stored through `appsettings.json` and ASP.NET Core configuration.

JWT configuration includes:

- Key
- Issuer
- Audience
- ExpireMinutes

Never commit real production secrets.

Do not expose JWT keys, passwords, connection-string secrets, or credentials in responses or logs.

Production secrets should eventually use environment variables or another secure configuration source.

---

## 17. Coding Style

Match the existing CostWise coding style unless there is a real reason to change it.

Do not rewrite working code only because another style is possible.

Do not perform large formatting-only refactors during feature work.

Avoid overengineering.

Keep methods readable.

Prefer obvious code over clever code.

Preserve existing naming unless changing it provides a real benefit and will not create unnecessary migration/database work.

---

## 18. Working With Existing Code

Before modifying code:

1. Read `PROJECT.md`.
2. Inspect the relevant Controller.
3. Inspect its Interface.
4. Inspect its Service.
5. Inspect related DTOs and Models.
6. Inspect ApplicationDbContext if database behavior is involved.
7. Understand the existing flow before proposing changes.

Do not guess how the project works from filenames alone.

---

## 19. Required Agent Workflow

For non-trivial tasks, follow this workflow:

Understand
-> Plan
-> Implement
-> Review Diff
-> Build
-> Test when possible
-> Summarize

Before editing:

1. Explain what you found.
2. Identify the files that need changes.
3. Explain the planned change.
4. Mention any security/database impact.

Then make the smallest necessary change.

After editing:

1. Review the diff.
2. Check for architecture violations.
3. Check for duplicated logic.
4. Check DTO usage.
5. Check authentication and ownership.
6. Check database changes.
7. Check error handling.
8. Build the project.
9. Report build errors or warnings honestly.
10. Summarize exactly what changed.

Never claim a build or test succeeded unless it was actually run successfully.

---

## 20. Review Checklist

When reviewing code, specifically check for:

- Business logic inside Controllers
- Direct DbContext usage inside Controllers
- Missing UserId ownership filters
- Trusting UserId from request DTOs
- Exposing EF entities unnecessarily
- Missing validation
- Incorrect HTTP status codes
- Returning null for multiple unrelated failure reasons
- Duplicate database queries
- Missing AsNoTracking on read-only entity queries
- AsNoTracking incorrectly used for tracked updates
- Unnecessary Include calls
- Security issues
- Password handling issues
- JWT mistakes
- Destructive database changes
- unnecessary migrations
- duplicated logic

Report issues before automatically rewriting large parts of the project.

---

## 21. Learning Project Rule

The developer is actively learning ASP.NET Core.

Do not hide important logic behind unnecessary frameworks or abstractions.

When introducing a new concept:

- explain why it is needed
- explain how it fits the existing architecture
- prefer understandable implementation
- do not replace learning opportunities with unexplained generated code

AI should assist development, not replace understanding.

The developer should be able to explain every important part of the final project.

---

## 22. Main Rule

DO SHIT CLEAN WAY.

Clean does not mean complicated.

Clean means:

- understandable
- secure
- maintainable
- consistent
- no unnecessary architecture
- no unnecessary code