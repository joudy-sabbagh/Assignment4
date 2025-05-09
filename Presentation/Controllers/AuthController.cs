// Presentation/Controllers/AuthController.cs
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Presentation.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _auth;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService auth, ILogger<AuthController> logger)
        {
            _auth = auth;
            _logger = logger;
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Login() => View();

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest m)
        {
            _logger.LogInformation("Login attempt for {Email}", m.Email);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Login model invalid for {Email}", m.Email);
                return View(m);
            }
            var token = await _auth.GenerateJwtTokenAsync(m);
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Invalid credentials for {Email}", m.Email);
                ModelState.AddModelError("", "Invalid credentials");
                return View(m);
            }
            _logger.LogInformation("Login successful for {Email}", m.Email);
            Response.Cookies.Append("X-Access-Token", token, new CookieOptions { HttpOnly = true, Secure = true });
            return RedirectToAction("Index", "Home");
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest m)
        {
            _logger.LogInformation("Registration attempt for {Email}", m.Email);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Registration model invalid for {Email}", m.Email);
                return View(m);
            }
            var res = await _auth.RegisterAsync(m);
            if (!res.Succeeded)
            {
                var errors = string.Join("; ", res.Errors.Select(e => e.Description));
                _logger.LogWarning("Registration failed for {Email}: {Errors}", m.Email, errors);
                foreach (var e in res.Errors) ModelState.AddModelError("", e.Description);
                return View(m);
            }
            _logger.LogInformation("Registration successful for {Email}", m.Email);
            var token = await _auth.GenerateJwtTokenAsync(new LoginRequest { Email = m.Email, Password = m.Password });
            Response.Cookies.Append("X-Access-Token", token, new CookieOptions { HttpOnly = true, Secure = true });
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _logger.LogInformation("Logout for {User}", User.Identity.Name);
            Response.Cookies.Delete("X-Access-Token");
            return RedirectToAction("Index", "Home");
        }
    }
}
