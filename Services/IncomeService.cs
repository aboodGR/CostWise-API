using CostWise_API.Data;
using CostWise_API.Interfaces;
using CostWise_API.Models;
using Microsoft.EntityFrameworkCore;

namespace CostWise_API.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly ApplicationDbContext _context;
        public IncomeService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Income> AddIncome(Income income)
        {
            await _context.Income.AddAsync(income);
            await _context.SaveChangesAsync();
            return income;
        }

        public async Task<bool> DeleteIncome(int Id)
        {
            var delete = await _context.Income.FirstOrDefaultAsync(c => c.Id == Id);
            if (delete == null)
            {
                return false;
            }
            _context.Income.Remove(delete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Income>> GetAllIncome()
        {
            return await _context.Income.AsNoTracking().ToListAsync();
        }

        public async Task<Income?> GetIncomeById(int Id)
        {
            var getId = await _context.Income.AsNoTracking().FirstOrDefaultAsync(c => c.Id == Id);
            return getId;
        }

        public async Task<Income?> UpdateIncome(int Id, Income income)
        {
            var idChecker = await _context.Income.FirstOrDefaultAsync(c => c.Id == Id);
            if (idChecker == null)
            {
                return null;
            }
            idChecker.Title = income.Title;
            idChecker.Amount = income.Amount;
            idChecker.Date = income.Date;
            idChecker.Description = income.Description;
            await _context.SaveChangesAsync();
            return idChecker;
        }
    }
}
