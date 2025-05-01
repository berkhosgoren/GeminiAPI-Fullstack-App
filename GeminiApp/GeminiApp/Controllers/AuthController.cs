using DocumentFormat.OpenXml.Math;
using GeminiApp.Data;
using GeminiApp.DTOs;
using GeminiApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GeminiApp.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace GeminiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenHelper _jwtTokenService;
        private readonly GeminiDbContext _context;

        public AuthController(JwtTokenHelper jwtTokenService, GeminiDbContext context)
        {
            _jwtTokenService = jwtTokenService;
            _context = context;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDTO userDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = _jwtTokenService.GenerateJwtToken(user);
            return Ok(new { Token = token, Username = user.Username, UserId=user.Id }); 
        }

    }
}
