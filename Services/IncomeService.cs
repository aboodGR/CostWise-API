using CostWise_API.Data;
using CostWise_API.Interfaces;
using CostWise_API.Models;

namespace CostWise_API.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly ApplicationDbContext _context;
        public IncomeService(ApplicationDbContext context)
        {
            _context = context;
        }
        public Income AddIncome(Income income)
        {
            
            _context.Income.Add(income);
            _context.SaveChanges();
            return income;
        }

        public bool DeleteIncome(int Id)
        {
            var delete = _context.Income.FirstOrDefault(c => c.Id == Id);
            if (delete == null)
            {
                return false;
            }
            _context.Income.Remove(delete);
            _context.SaveChanges();
            return true;
        }

        public List<Income> GetAllIncome()
        {
            return _context.Income.ToList();
        }

        public Income? GetIncomeById(int Id)
        {
            var getId = _context.Income.FirstOrDefault(c => c.Id == Id);
            return getId;
        }

        public Income? UpdateIncome(int Id, Income income)
        {
            var idChecker = _context.Income.FirstOrDefault(c => c.Id == Id);
            if (idChecker == null)
            {
                return null;
            }
            idChecker.Title = income.Title;
            idChecker.Amount = income.Amount;
            idChecker.Date = income.Date;
            idChecker.Description = income.Description;
            _context.SaveChanges();
            return idChecker;
        }
    }
}
