using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CostWise_API.Models
{
    public class Expense
    {
        
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public DateOnly Date { get; set; }
        public string Description { get; set; }
        
    }
}
