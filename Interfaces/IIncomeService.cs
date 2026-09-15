using CostWise_API.Models;

namespace CostWise_API.Interfaces
{
    public interface IIncomeService
    {
        List<Income> GetAllIncome();
        Income? GetIncomeById(int Id);
        Income AddIncome(Income income);
        Income? UpdateIncome(int Id, Income income);
        bool DeleteIncome(int Id);
    }
}
