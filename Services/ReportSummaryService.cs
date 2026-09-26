using CostWise_API.Configuration;
using CostWise_API.Data;
using CostWise_API.Interfaces;
using CostWise_API.Models;
using Microsoft.EntityFrameworkCore;
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


        public async Task<ReportSummary> GetSummary()
        {

            var amountExp = await _context.Expense.AnyAsync()
                ? await _context.Expense.SumAsync(c => c.Amount)
                : 0;
            var amountInc = await _context.Income.AnyAsync()
                ? await _context.Income.SumAsync(c=>c.Amount)
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
