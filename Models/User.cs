using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WalMart_ERP_System.Models
{
    public class User
    {
        
        public int Id { get; set; }
        [EmailAddress]
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string PasswordHash { get; set; }
    }
}
