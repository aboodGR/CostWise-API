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

Technologies

* C#
* ASP.NET Core
* .NET
* LINQ
* Dependency Injection

Project Structure

Controllers
Interfaces
Services
Models
Configuration
Middleware

I separated the controllers, interfaces, services, and models to keep the project organized.


Storage

For now, the project uses in-memory lists to store the data.

I will connect it to SQL Server using Entity Framework Core later.

Running the project

Clone the repository and open the project.

Then run:

dotnet run


You can test the API using Swagger or Postman.

Project Status

This is still a work in progress and I'm building it while learning ASP.NET Core.

The next step is connecting the API to a real database and improving the project further.
