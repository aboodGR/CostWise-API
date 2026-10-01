using CostWise_API.DTOs.Auth;
using CostWise_API.Interfaces;
using CostWise_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CostWise_API.Controllers
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
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAc(RegisterDto registerDto) {
            var results = await _authService.Register(registerDto);
            if (results == false)
                return BadRequest();
            return Ok(results);
        }
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
