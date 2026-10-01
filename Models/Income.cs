namespace CostWise_API.Models
{
    public class Income
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateOnly Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public int UserId { get; set; }
        public User User { get; set; } = null!;

    }
}
