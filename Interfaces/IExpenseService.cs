using CostWise_API.Models;

namespace CostWise_API.Interfaces
{
    public interface IExpenseService
    {
        Task<List<Expense>> GetAllExpenses(int userId);
        Task<Expense?> GetExpenseById(int Id , int userId);
        Task<Expense?> AddExpense(Expense expense);
        Task<Expense?> UpdateExpense(int Id , Expense expense, int userId);
        Task<bool> DeleteExpense(int Id, int userId);
    }
}
