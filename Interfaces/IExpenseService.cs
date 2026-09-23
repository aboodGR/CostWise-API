using CostWise_API.Models;

namespace CostWise_API.Interfaces
{
    public interface IExpenseService
    {
        List<Expense> GetAllExpenses();
        Expense? GetExpenseById(int Id);
        Expense? AddExpense(Expense expense);
        Expense? UpdateExpense(int Id , Expense expense);
        bool DeleteExpense(int Id);
    }
}
