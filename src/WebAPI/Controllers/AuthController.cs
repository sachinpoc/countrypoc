using Microsoft.AspNetCore.Mvc;
using MyCleanArchitectureApp.Core.Interfaces;
using System.Threading.Tasks;

namespace MyCleanArchitectureApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username)
        {

            var token = await _tokenService.GenerateToken(username);
            return Ok(new { Token = token });
        }
    }
}
