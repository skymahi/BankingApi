using Microsoft.AspNetCore.Mvc;
using BankingApi.Services;
using BankingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly BankingDbContext _context;

        public AuthController(JwtService jwtService, BankingDbContext context)
        {
            _jwtService = jwtService;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (_context.Users.Any(x => x.Name == loginRequest.Username && x.Password == loginRequest.Password))
            {
                var user = new User { Name = loginRequest.Username };
                var token = _jwtService.GenerateToken(user);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid credentials.");
        }
    }
}
