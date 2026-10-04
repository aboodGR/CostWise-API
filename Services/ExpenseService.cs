using CostWise_API.Data;
using CostWise_API.Interfaces;
using CostWise_API.Models;
using CostWise_API.Results;
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
            var category = await _context.Category.FirstOrDefaultAsync(x=>x.Id == expense.CategoryId && x.UserId == expense.UserId);
            if (category != null) {
                expense.Category = category;
                await _context.Expense.AddAsync(expense);
                await _context.SaveChangesAsync();
                return expense;
            }
            return null;
            
        }

        public async Task<bool> DeleteExpense(int Id, int userId)
        {
            var delete = await _context.Expense.FirstOrDefaultAsync(c => c.Id == Id && c.UserId == userId);
            if (delete == null)
            {
                return false;
            }
            _context.Expense.Remove(delete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Expense>> GetAllExpenses(int userId)
        {
            var expense = await _context.Expense
                .Where(x=>x.UserId==userId)
                .Include(x => x.Category)
                .AsNoTracking()
                .ToListAsync();
            return expense;
        }

        public async Task<Expense?> GetExpenseById(int Id, int userId)
        {

            var getId = await _context.Expense
                .Include(x=>x.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == Id && c.UserId == userId);
            return getId;
        }

        public async Task<Result<Expense>> UpdateExpense(int Id, Expense expense, int userId)
        {
            var idChecker = await _context.Expense.FirstOrDefaultAsync(c => c.Id == Id && c.UserId == userId);
            if (idChecker == null)
            {
                return new Result<Expense> {
                    Success = false,
                    ErrorCode = "ExpenseNotFound",
                    ErrorMessage = "Expense Not Found"
                };
            }
            var category = await _context.Category.FirstOrDefaultAsync(x => x.Id == expense.CategoryId && x.UserId == userId);
            if (category != null) {
                idChecker.Title = expense.Title;
                idChecker.Amount = expense.Amount;
                idChecker.CategoryId = expense.CategoryId;
                idChecker.Category = category;
                idChecker.Date = expense.Date;
                idChecker.Description = expense.Description;
                await _context.SaveChangesAsync();
                return new Result<Expense>
                {
                    Success=true,
                    Data = idChecker
                };
            }
            return new Result<Expense> {
                Success = false,
                ErrorCode = "CategoryNotFound",
                ErrorMessage = "Category Not Found"
            };
            
        }
    }
}
