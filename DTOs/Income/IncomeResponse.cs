namespace CostWise_API.DTOs.Income
{
    public class IncomeResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateOnly Date { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
