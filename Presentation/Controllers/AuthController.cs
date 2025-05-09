// Presentation/Controllers/AuthController.cs
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth) => _auth = auth;

        [HttpGet, AllowAnonymous]
        public IActionResult Login() => View();

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest m)
        {
            if (!ModelState.IsValid) return View(m);
            var token = await _auth.GenerateJwtTokenAsync(m);
            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError("", "Invalid credentials");
                return View(m);
            }
            Response.Cookies.Append("X-Access-Token", token, new CookieOptions { HttpOnly = true, Secure = true });
            return RedirectToAction("Index", "Home");
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest m)
        {
            if (!ModelState.IsValid) return View(m);
            var res = await _auth.RegisterAsync(m);
            if (!res.Succeeded)
            {
                foreach (var e in res.Errors) ModelState.AddModelError("", e.Description);
                return View(m);
            }
            var token = await _auth.GenerateJwtTokenAsync(new LoginRequest { Email = m.Email, Password = m.Password });
            Response.Cookies.Append("X-Access-Token", token, new CookieOptions { HttpOnly = true, Secure = true });
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("X-Access-Token");
            return RedirectToAction("Index", "Home");
        }
    }
}
