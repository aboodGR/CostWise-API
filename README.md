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

This project is still in progress.

The API is now connected to SQL Server using Entity Framework Core, and the main services are using the database instead of in-memory lists.

I also added the relationship between Categories and Expenses and started loading related data with EF Core.

Next I will continue improving the controllers, validation, and the rest of the backend features.
