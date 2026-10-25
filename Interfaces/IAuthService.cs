using WalMart_ERP_System.DTOs.Auth;

namespace WalMart_ERP_System.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Register(RegisterDto registerDto);
        Task<LoginResponseDto?> Login(LoginDto loginDto);
    }
}
