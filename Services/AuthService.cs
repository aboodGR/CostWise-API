using CostWise_API.Data;
using CostWise_API.DTOs.Auth;
using CostWise_API.Interfaces;
using CostWise_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CostWise_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;
        public AuthService(ApplicationDbContext context , IPasswordHasher<User> passwordHasher , IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }
        public async Task<bool> Register(RegisterDto registerDto) {
            var emailExists = await _context.User.AnyAsync(x => x.Email==registerDto.Email);
            if (emailExists) {
                return false;
            }
            var user = new User{
                Email = registerDto.Email,
                PasswordHash = string.Empty
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;

        }

        #region TokenLogin
        public async Task<LoginResponseDto?> Login(LoginDto loginDto) {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Email == loginDto.Email);
            if (user == null)
                return null;
            var result = _passwordHasher.VerifyHashedPassword(user,user.PasswordHash,loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
                return null;
            var claims = new List<Claim>{
                new Claim("UserId",user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email)
            };
            var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
                );
            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );
            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(
                        int.Parse(_configuration["Jwt:ExpireMinutes"]!)
                    ),
                    signingCredentials: credentials
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return new LoginResponseDto { 
                Token=tokenString,
                Email=loginDto.Email
            };
        }
        #endregion
    }
}
