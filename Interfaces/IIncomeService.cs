using CostWise_API.Models;

namespace CostWise_API.Interfaces
{
    public interface IIncomeService
    {
        Task<List<Income>> GetAllIncome();
        Task<Income?> GetIncomeById(int Id);
        Task<Income> AddIncome(Income income);
        Task<Income?> UpdateIncome(int Id, Income income);
        Task <bool> DeleteIncome(int Id);
    }
}
