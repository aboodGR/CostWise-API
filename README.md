CostWise API
CostWise API is a personal finance API that I made using ASP.NET Core.
I made this project to practice what I have been learning in ASP.NET Core, especially controllers, services, interfaces, dependency injection, middleware, configuration, and working with APIs.


What it does
Right now the API has:
* Categories
* Expenses
* Income
* CRUD operations
* Expense and income summary
* Configuration using the Options Pattern
* Request timing middleware
* Connected to SMSS Local DB

Technologies
* C#
* ASP.NET Core
* .NET
* LINQ
* Entity Framework

Running the project
Clone the repository and open the project.

Then run:
dotnet run
You can test the API using Swagger or Postman.

Project Status

Completed so far:

- ASP.NET Core Web API structure
- Dependency Injection
- Configuration with appsettings.json
- Entity Framework Core with SQL Server
- Code First migrations
- Category, Expense, and Income CRUD
- Category to Expense relationship
- Foreign key validation for expenses
- Related data loading using Include
- Read-only queries using AsNoTracking
- EF Core async database operations
- Async service and controller methods
