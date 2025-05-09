// Presentation/Controllers/AuthController.cs
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            // 1) check if the email is already taken
            var existing = await _userManager.FindByEmailAsync(model.Email);
            if (existing != null)
                return BadRequest(new { message = "Email is already registered." });

            // 2) create the user
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth
            };
            var createResult = await _userManager.CreateAsync(user, model.Password);
            if (!createResult.Succeeded)
                return BadRequest(createResult.Errors);

            // 3) ensure the "User" role exists
            const string userRole = "User";
            if (!await _roleManager.RoleExistsAsync(userRole))
            {
                var role = new ApplicationRole
                {
                    Name = userRole,
                    Description = "Standard application user"
                };
                await _roleManager.CreateAsync(role);
            }

            // 4) assign the role
            await _userManager.AddToRoleAsync(user, userRole);

            return Ok(new { message = "Registration successful." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            // 1) verify credentials
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized(new { message = "Invalid email or password." });

            // 2) get user roles
            var roles = await _userManager.GetRolesAsync(user);

            // 3) build JWT claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,    user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,  user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,    Guid.NewGuid().ToString())
            };
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            // 4) read JWT settings
            var jwtSection = _configuration.GetSection("Jwt");
            var keyBytes = Encoding.UTF8.GetBytes(jwtSection["Key"]!);
            var creds = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256
            );
            var expires = DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(jwtSection["ExpireMinutes"])
            );

            // 5) create the token
            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
    }

    // DTOs used by AuthController
    public class RegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
