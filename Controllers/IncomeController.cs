using CostWise_API.DTOs;
using CostWise_API.DTOs.Income;
using CostWise_API.Interfaces;
using CostWise_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CostWise_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
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
            var userIdClaim = User.FindFirst("UserId")?.Value;
            var userId = int.Parse(userIdClaim);
            var incomes = await income.GetAllIncome(userId);
            var response = incomes.Select(x => new IncomeResponse
            {
                Id = x.Id,
                Title = x.Title,
                Amount = x.Amount,
                Date = x.Date,
                Description = x.Description
            });
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIncomeByIdAc(int id)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            var userId = int.Parse(userIdClaim);
            var result = await income.GetIncomeById(id, userId);
            if (result == null)
                return NotFound();
            var response = new IncomeResponse
            {
                Id = result.Id,
                Title = result.Title,
                Amount = result.Amount,
                Date = result.Date,
                Description = result.Description
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> AddIncomeAc(CreateIncomeDto createIncomeDto)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            var newIncome = new Income
            {
                Title = createIncomeDto.Title,
                Amount = createIncomeDto.Amount,
                Date = createIncomeDto.Date,
                Description = createIncomeDto.Description,
                UserId = int.Parse(userIdClaim)
            };
            var result = await income.AddIncome(newIncome);
            if (result == null) {
                return BadRequest();
            }
            return Ok(new IncomeResponse
            {
                Id = result.Id,
                Title = result.Title,
                Amount = result.Amount,
                Date = result.Date,
                Description = result.Description
            });
        }
        [HttpPut]
        public async Task<IActionResult> UpdateIncomeAc(int id, UpdateIncomeDto updateIncomeDto)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            var userId = int.Parse(userIdClaim);
            var newIncome = new Income
            {
                Title = updateIncomeDto.Title,
                Amount = updateIncomeDto.Amount,
                Date = updateIncomeDto.Date,
                Description = updateIncomeDto.Description
            };
            var updated = await income.UpdateIncome(id, newIncome, userId);
            if (updated == null)
                return NotFound();
            return Ok(new IncomeResponse
            {
                Id = updated.Id,
                Title = updated.Title,
                Amount = updated.Amount,
                Date = updated.Date,
                Description = updated.Description
            });
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteIncomeAc(int id)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            var userId = int.Parse(userIdClaim);
            var deleted = await this.income.DeleteIncome(id, userId);
            if (deleted == false)
                return NotFound();
            return Ok(deleted);
        }
    }
}
