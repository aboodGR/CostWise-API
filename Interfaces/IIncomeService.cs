using CostWise_API.Models;

namespace CostWise_API.Interfaces
{
    public interface IIncomeService
    {
        Task<List<Income>> GetAllIncome(int userId);
        Task<Income?> GetIncomeById(int Id, int userId);
        Task<Income> AddIncome(Income income);
        Task<Income?> UpdateIncome(int Id, Income income, int userId);
        Task <bool> DeleteIncome(int Id, int userId);
    }
}
