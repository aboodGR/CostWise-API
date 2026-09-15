using CostWise_API.Configuration;
using CostWise_API.Interfaces;
using CostWise_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<ICategoryService, CategoryService>();
builder.Services.AddSingleton<IExpenseService, ExpenseService>();
builder.Services.AddSingleton<IIncomeService, IncomeService>();
builder.Services.AddSingleton<IReportService, ReportSummaryService>();

builder.Services.Configure<CostWiseSettings>(
    builder.Configuration.GetSection("CostWiseSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
