using CostWise_API.Configuration;
using CostWise_API.Interfaces;
using CostWise_API.Models;
using Microsoft.Extensions.Options;

namespace CostWise_API.Services
{
    public class ReportSummaryService : IReportService
    {
        private readonly IExpenseService expenseService;
        private readonly IIncomeService incomeService;
        private readonly CostWiseSettings settings;

        public ReportSummaryService(IExpenseService expenseService, IIncomeService incomeService, IOptions<CostWiseSettings> options)
        {
            this.expenseService = expenseService;
            this.incomeService = incomeService;
            settings = options.Value;
        }


        public ReportSummary GetSummary()
        {
            decimal amountExp = expenseService.GetAllExpenses().Sum(c => c.Amount);

            decimal amountInc = incomeService.GetAllIncome().Sum(c => c.Amount);

            decimal TotalAmmount = amountInc - amountExp;

            return new ReportSummary
            {
                TotalIncome = amountInc,
                TotalExpenses = amountExp,
                Balance = TotalAmmount
            };

        }
        //trhis is a temp method just to read the values of the confg
        public string GetSettings()
        {
            return $"Currency: {settings.Currency}, Max Expense: {settings.MaxExpenseAmount}";
        }


    }
}
