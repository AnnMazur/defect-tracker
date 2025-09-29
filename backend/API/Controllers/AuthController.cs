using System.Threading;
using System.Threading.Tasks;
using defectTracker.DTOs;
using defectTracker.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace defectTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto, CancellationToken cancellationToken)
        {
            var response = await _authService.RegisterAsync(dto, cancellationToken);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto, CancellationToken cancellationToken)
        {
            var response = await _authService.LoginAsync(dto, cancellationToken);
            return Ok(response);
        }
    }
}
