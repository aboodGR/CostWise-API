using WalMart_ERP_System.DTOs.Auth;
using WalMart_ERP_System.Interfaces;
using WalMart_ERP_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WalMart_ERP_System.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAc(RegisterDto registerDto) {
            var results = await _authService.Register(registerDto);
            if (results == false)
                return BadRequest();
            return Ok(results);
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAc(LoginDto loginDto)
        {
            var results = await _authService.Login(loginDto);
            if (results == null)
                return Unauthorized();
            return Ok(results);
        }
    }
}
