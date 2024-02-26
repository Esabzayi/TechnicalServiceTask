using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechnicalServiceTask.Data;
using TechnicalServiceTask.Models;

namespace TechnicalServiceTask.Controllers.jwtAuth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppEntity _context;

        public AuthController(AppEntity context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Username is already taken");
                return BadRequest(ModelState);
            }

            var passwordHash = HashPassword(model.Password);

            var newUser = new User
            {
                Username = model.Username,
                PasswordHash = passwordHash,
                // Add other properties as needed
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(newUser.Id.ToString());

            return Ok(new { Token = token });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserViewModel model)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == model.Username);

            if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("InvalidCredentials", "Invalid username or password");
                return BadRequest(ModelState);
            }

            var token = GenerateJwtToken(user.Id.ToString());

            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(string userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userId),
                // Add other claims as needed
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key-with-at-least-256-bits"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                "http://localhost",
                "http://localhost:5297",
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string inputPassword, string hashedPassword)
        {

            return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
        }
    }
}
