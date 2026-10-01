using CostWise_API.DTOs.Auth;

namespace CostWise_API.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Register(RegisterDto registerDto);
        Task<LoginResponseDto?> Login(LoginDto loginDto);
    }
}
