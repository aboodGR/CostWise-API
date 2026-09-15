using CostWise_API.Interfaces;
using CostWise_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CostWise_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService expense;
        public ExpenseController(IExpenseService expense)
        {
            this.expense = expense;
        }

        [HttpGet]
        public IActionResult GetAllExpenseAc()
        {
            return Ok(expense.GetAllExpenses());
        }
        [HttpGet("{id}")]
        public IActionResult GetExpenseByIdAc(int id)
        {
            return Ok(expense.GetExpenseById(id));
        }
        [HttpPost]
        public IActionResult AddExpenseAc(Expense expense)
        {
            return Ok(this.expense.AddExpense(expense));
        }
        [HttpPut]
        public IActionResult UpdateExpenseAc(int id, Expense expense)
        {
            var updated = this.expense.UpdateExpense(id, expense);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }
        [HttpDelete]
        public IActionResult DeleteexpenseAc(int id)
        {
            var deleted = this.expense.DeleteExpense(id);
            if (deleted == false)
                return NotFound();
            return Ok(deleted);
        }
    }
}
