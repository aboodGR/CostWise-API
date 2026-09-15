using CostWise_API.Interfaces;
using CostWise_API.Models;

namespace CostWise_API.Services
{
    public class ExpenseService : IExpenseService
    {
        List<Expense> listExpense = new List<Expense>();
        int idCounter = 0;
        public Expense AddExpense(Expense expense)
        {
            idCounter++;
            expense.Id = idCounter;
            listExpense.Add(expense);
            return expense;
        }

        public bool DeleteExpense(int Id)
        {
            var delete = listExpense.FirstOrDefault(c => c.Id == Id);
            if (delete == null)
            {
                return false;
            }
            listExpense.Remove(delete);
            return true;
        }

        public List<Expense> GetAllExpenses()
        {
            return listExpense;
        }

        public Expense? GetExpenseById(int Id)
        {
            var getId = listExpense.FirstOrDefault(c => c.Id == Id);
            return getId;
        }

        public Expense? UpdateExpense(int Id, Expense expense)
        {
            var idChecker = listExpense.FirstOrDefault(c => c.Id == Id);
            if (idChecker == null)
            {
                return null;
            }
            expense.Id = idChecker.Id;
            listExpense.Remove(idChecker);
            listExpense.Add(expense);
            return expense;
        }
    }
}
