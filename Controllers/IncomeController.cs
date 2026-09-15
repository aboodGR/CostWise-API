using CostWise_API.Interfaces;
using CostWise_API.Models;
using CostWise_API.Services;
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
        public IActionResult GetAllIncomeAc()
        {
            return Ok(income.GetAllIncome());
        }
        [HttpGet("{id}")]
        public IActionResult GetIncomeByIdAc(int id)
        {
            return Ok(income.GetIncomeById(id));
        }
        [HttpPost]
        public IActionResult AddIncomeAc(Income income)
        {
            return Ok(this.income.AddIncome(income));
        }
        [HttpPut]
        public IActionResult UpdateIncomeAc(int id, Income income)
        {
            var updated = this.income.UpdateIncome(id, income);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }
        [HttpDelete]
        public IActionResult DeleteIncomeAc(int id)
        {
            var deleted = this.income.DeleteIncome(id);
            if (deleted == false)
                return NotFound();
            return Ok(deleted);
        }
    }
}
