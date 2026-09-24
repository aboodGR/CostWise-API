using CostWise_API.Data;
using CostWise_API.Interfaces;
using CostWise_API.Models;
using Microsoft.EntityFrameworkCore;

namespace CostWise_API.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly ApplicationDbContext _context;
        public ExpenseService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Expense?> AddExpense(Expense expense)
        {
            var check = await _context.Category.AnyAsync(x=>x.Id == expense.CategoryId);
            if (check) {
                await _context.Expense.AddAsync(expense);
                await _context.SaveChangesAsync();
                return expense;
            }
            return null;
            
        }

        public async Task<bool> DeleteExpense(int Id)
        {
            var delete = await _context.Expense.FirstOrDefaultAsync(c => c.Id == Id);
            if (delete == null)
            {
                return false;
            }
            _context.Expense.Remove(delete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Expense>> GetAllExpenses()
        {
            var expense = await _context.Expense
                .Include(x => x.Category)
                .AsNoTracking()
                .ToListAsync();
            return expense;
        }

        public async Task<Expense?> GetExpenseById(int Id)
        {
            var getId = await _context.Expense
                .Include(x=>x.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == Id);
            return getId;
        }

        public async Task<Expense?> UpdateExpense(int Id, Expense expense)
        {
            var idChecker = await _context.Expense.FirstOrDefaultAsync(c => c.Id == Id);
            if (idChecker == null)
            {
                return null;
            }
            var check = await _context.Category.AnyAsync(x => x.Id == expense.CategoryId);
            if (check) {
                idChecker.Title = expense.Title;
                idChecker.Amount = expense.Amount;
                idChecker.CategoryId = expense.CategoryId;
                idChecker.Date = expense.Date;
                idChecker.Description = expense.Description;
                await _context.SaveChangesAsync();
                return idChecker;
            }
            return null;
            
        }
    }
}
