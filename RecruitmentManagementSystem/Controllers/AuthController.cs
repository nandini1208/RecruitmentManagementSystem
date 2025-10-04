using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentManagementSystem.Data;
using RecruitmentManagementSystem.DTOs;
using RecruitmentManagementSystem.Models;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecruitmentManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupDto signupDto)
        {
            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Email == signupDto.Email))
            {
                return BadRequest("User with this email already exists");
            }

            // Create new user
            var user = new User
            {
                Name = signupDto.Name,
                Email = signupDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(signupDto.Password),
                Address = signupDto.Address,
                UserType = (UserType)Enum.Parse(typeof(UserType), signupDto.UserType),
                ProfileHeadline = signupDto.ProfileHeadline
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Create empty profile for the user with ALL required fields
            var profile = new Profile
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                ResumeFileAddress = "",
                Skills = "",
                Education = "",
                Experience = "",
                Phone = ""
            };

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User created successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password");
            }

            // Generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.UserType.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Message = "Login successful",
                Token = tokenString,
                UserType = user.UserType.ToString(),
                UserId = user.Id
            });
        }
    }
}