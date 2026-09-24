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
        public async Task<IActionResult> GetAllExpenseAc()
        {
            return Ok(await expense.GetAllExpenses());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpenseByIdAc(int id)
        {
            var result = await expense.GetExpenseById(id);
            if (result == null) {
                return NotFound();
            }
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> AddExpenseAc(Expense expense)
        {
            var result = await this.expense.AddExpense(expense);
            if (result == null) {
                return BadRequest("Category does not exist.");
            }
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateExpenseAc(int id, Expense expense)
        {
            var updated = await this.expense.UpdateExpense(id, expense);
            if (updated == null)
                return BadRequest();
            return Ok(updated);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteexpenseAc(int id)
        {
            var deleted = await this.expense.DeleteExpense(id);
            if (deleted == false)
                return NotFound();
            return Ok(deleted);
        }
    }
}
