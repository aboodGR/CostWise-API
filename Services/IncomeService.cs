using CostWise_API.Interfaces;
using CostWise_API.Models;

namespace CostWise_API.Services
{
    public class IncomeService : IIncomeService
    {
        List<Income> listIncome = new List<Income>();
        int idCounter = 0;
        public Income AddIncome(Income income)
        {
            idCounter++;
            income.Id = idCounter;
            listIncome.Add(income);
            return income;
        }

        public bool DeleteIncome(int Id)
        {
            var delete = listIncome.FirstOrDefault(c => c.Id == Id);
            if (delete == null)
            {
                return false;
            }
            listIncome.Remove(delete);
            return true;
        }

        public List<Income> GetAllIncome()
        {
            return listIncome;
        }

        public Income? GetIncomeById(int Id)
        {
            var getId = listIncome.FirstOrDefault(c => c.Id == Id);
            return getId;
        }

        public Income? UpdateIncome(int Id, Income income)
        {
            var idChecker = listIncome.FirstOrDefault(c => c.Id == Id);
            if (idChecker == null)
            {
                return null;
            }
            income.Id = idChecker.Id;
            listIncome.Remove(idChecker);
            listIncome.Add(income);
            return income;
        }
    }
}
