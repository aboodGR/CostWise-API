using System.ComponentModel.DataAnnotations;

namespace WalMart_ERP_System.DTOs.Auth
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
