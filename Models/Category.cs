using System.ComponentModel.DataAnnotations;

namespace CostWise_API.Models
{
    public class Category
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Expense> Expense { get; set; } = new List<Expense>();

    }
}
