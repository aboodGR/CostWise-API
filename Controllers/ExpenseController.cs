using CostWise_API.DTOs.Expense;
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
            var expenses = await expense.GetAllExpenses();
            var response = expenses.Select(x => new ExpenseResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                Amount = x.Amount,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                Date = x.Date,
                Description = x.Description
            });
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpenseByIdAc(int id)
        {
            var result = await expense.GetExpenseById(id);
            if (result == null) {
                return NotFound();
            }
            var responseDto = new ExpenseResponseDto
            {
                Id = result.Id,
                Title = result.Title,
                Amount = result.Amount,
                CategoryId = result.CategoryId,
                CategoryName = result.Category.Name,
                Date = result.Date,
                Description = result.Description
            };
            return Ok(responseDto);
        }
        [HttpPost]
        public async Task<IActionResult> AddExpenseAc(CreateExpenseDto createExpenseDto)
        {
            var newExpense = new Expense {
                Title = createExpenseDto.Title,
                Amount = createExpenseDto.Amount,
                CategoryId = createExpenseDto.CategoryId,
                Date = createExpenseDto.Date,
                Description = createExpenseDto.Description
            };
            var result = await expense.AddExpense(newExpense);
            if (result == null) {
                return BadRequest("Category does not exist.");
            }
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateExpenseAc(int id, UpdateExpenseDto updateExpenseDto)
        {
            var updatedExpense = new Expense {
                Title = updateExpenseDto.Title,
                Amount = updateExpenseDto.Amount,
                CategoryId = updateExpenseDto.CategoryId,
                Date = updateExpenseDto.Date,
                Description = updateExpenseDto.Description
            };
            var updated = await this.expense.UpdateExpense(id, updatedExpense);
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
