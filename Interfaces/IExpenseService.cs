using CostWise_API.Models;

namespace CostWise_API.Interfaces
{
    public interface IExpenseService
    {
        Task<List<Expense>> GetAllExpenses();
        Task<Expense?> GetExpenseById(int Id);
        Task<Expense?> AddExpense(Expense expense);
        Task<Expense?> UpdateExpense(int Id , Expense expense);
        Task<bool> DeleteExpense(int Id);
    }
}
