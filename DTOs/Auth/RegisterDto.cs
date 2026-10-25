using System.ComponentModel.DataAnnotations;

namespace WalMart_ERP_System.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public required string Email { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(100)]
        public required string Password { get; set; }

    }
}
