using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Data;
using PasswordManager.Api.Models;
using PasswordManager.Api.Models.DTOs;
using PasswordManager.Api.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PasswordManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IGoogleAuthService googleAuthService, IConfiguration configuration)
        {
            _context = context;
            _googleAuthService = googleAuthService;
            _configuration = configuration;
        }

        [HttpPost("google")]
        public async Task<ActionResult<AuthResponse>> GoogleAuth([FromBody] GoogleAuthRequest request)
        {
            if (string.IsNullOrEmpty(request.IdToken))
            {
                return BadRequest("ID token is required");
            }

            var googleUser = await _googleAuthService.ValidateGoogleTokenAsync(request.IdToken);
            if (googleUser == null)
            {
                return BadRequest("Invalid Google token");
            }

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.GoogleId == googleUser.GoogleId);

            bool isNewUser = false;
            User user;

            if (existingUser == null)
            {
                // Sign up - Create new user
                user = new User
                {
                    GoogleId = googleUser.GoogleId,
                    Email = googleUser.Email,
                    Name = googleUser.Name,
                    ProfilePictureUrl = googleUser.ProfilePictureUrl,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                isNewUser = true;
            }
            else
            {
                // Sign in - Update existing user info
                user = existingUser;
                user.Name = googleUser.Name;
                user.Email = googleUser.Email;
                user.ProfilePictureUrl = googleUser.ProfilePictureUrl;
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }

            var token = GenerateJwtToken(user);

            return Ok(new AuthResponse
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Token = token,
                IsNewUser = isNewUser
            });
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"] ?? "your-secret-key-here");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}