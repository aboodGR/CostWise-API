namespace CostWise_API.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public DateOnly Date { get; set; }
        public string Description { get; set; }
    }
}
