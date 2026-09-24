using CostWise_API.Configuration;
using CostWise_API.Data;
using CostWise_API.Interfaces;
using CostWise_API.Models;
using Microsoft.Extensions.Options;

namespace CostWise_API.Services
{
    public class ReportSummaryService : IReportService
    {
        private readonly ApplicationDbContext _context;
        private readonly CostWiseSettings settings;

        public ReportSummaryService(ApplicationDbContext context,IOptions<CostWiseSettings> options)
        {
            _context = context;
            settings = options.Value;
        }


        public ReportSummary GetSummary()
        {

            var amountExp = _context.Expense.Any().
                ? _context.Expense.Sum(c => c.Amount)
                : 0;
            var amountInc = _context.Income.Any()
                ? _context.Income.Sum(c=>c.Amount)
                : 0 ;
            decimal balance = amountInc - amountExp;

            return new ReportSummary
            {
                TotalIncome = amountInc,
                TotalExpenses = amountExp,
                Balance = balance
            };

        }
        //this is a temp method just to read the values of the confg
        public string GetSettings()
        {
            return $"Currency: {settings.Currency}, Max Expense: {settings.MaxExpenseAmount}";
        }


    }
}
