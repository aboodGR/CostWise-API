namespace CostWise_API.DTOs.Expense
{
    public class UpdateExpenseDto
    {
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public DateOnly Date { get; set; }
        public string Description { get; set; } = string.Empty;

    }
}
