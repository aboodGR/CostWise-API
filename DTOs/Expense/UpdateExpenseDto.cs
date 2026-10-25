using System.ComponentModel.DataAnnotations;

namespace CostWise_API.DTOs.Expense
{
    public class UpdateExpenseDto
    {
        [Required]
        [MaxLength(20)]
        public string Title { get; set; } = string.Empty;
        [Range(typeof(decimal), "0.01", "100000")]
        public decimal Amount { get; set; }
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
        public DateOnly Date { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

    }
}
