using CostWise_API.Interfaces;
using CostWise_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CostWise_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService income;
        public IncomeController(IIncomeService income)
        {
            this.income = income;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIncomeAc()
        {
            return Ok(await income.GetAllIncome());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIncomeByIdAc(int id)
        {
            var result = await income.GetIncomeById(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddIncomeAc(Income income)
        {
            return Ok(await this.income.AddIncome(income));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateIncomeAc(int id, Income income)
        {
            var updated = await this.income.UpdateIncome(id, income);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteIncomeAc(int id)
        {
            var deleted = await this.income.DeleteIncome(id);
            if (deleted == false)
                return NotFound();
            return Ok(deleted);
        }
    }
}
