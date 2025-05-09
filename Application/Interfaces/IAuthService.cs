using System.Threading.Tasks;
using Application.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterRequest request);
        Task<string> GenerateJwtTokenAsync(LoginRequest request);
    }
}
