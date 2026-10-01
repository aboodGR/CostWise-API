using System.ComponentModel.DataAnnotations;

namespace CostWise_API.DTOs.Income
{
    public class UpdateIncomeDto
    {
        [Required]
        [MaxLength(20)]
        public string Title { get; set; } = string.Empty;
        [Range(typeof(decimal),"0.01","100000")]
        public decimal Amount { get; set; }
        public DateOnly Date { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
    }
}
