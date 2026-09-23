using CostWise_API.Data;
using CostWise_API.Interfaces;
using CostWise_API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace CostWise_API.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly ApplicationDbContext _context;
        public ExpenseService(ApplicationDbContext context)
        {
            _context = context;
        }
        public Expense? AddExpense(Expense expense)
        {
            var check = _context.Category.Any(x=>x.Id == expense.CategoryId);
            if (check) {
                _context.Expense.Add(expense);
                _context.SaveChanges();
                return expense;
            }
            return null;
            
        }

        public bool DeleteExpense(int Id)
        {
            var delete = _context.Expense.FirstOrDefault(c => c.Id == Id);
            if (delete == null)
            {
                return false;
            }
            _context.Expense.Remove(delete);
            _context.SaveChanges();
            return true;
        }

        public List<Expense> GetAllExpenses()
        {
            var expense = _context.Expense
                .Include(x => x.Category)
                .ToList();
            return expense;
        }

        public Expense? GetExpenseById(int Id)
        {
            var getId = _context.Expense
                .Include(x=>x.Category)
                .FirstOrDefault(c => c.Id == Id);
            return getId;
        }

        public Expense? UpdateExpense(int Id, Expense expense)
        {
            var idChecker = _context.Expense.FirstOrDefault(c => c.Id == Id);
            if (idChecker == null)
            {
                return null;
            }
            var check = _context.Category.Any(x => x.Id == expense.CategoryId);
            if (check) {
                idChecker.Title = expense.Title;
                idChecker.Amount = expense.Amount;
                idChecker.CategoryId = expense.CategoryId;
                idChecker.Date = expense.Date;
                idChecker.Description = expense.Description;
                _context.SaveChanges();
                return idChecker;
            }
            return null;
            
        }
    }
}
