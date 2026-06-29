using System.Security.Claims;
using WebApi.Models;

namespace WebApi.Services;
public interface IAuthenticationService<T>
{
    Task<T?> ValidateUserAsync(string email, string password);

    string CreateToken(T agent);

    string CreateRefreshToken(T agent);

    ClaimsPrincipal? ValidateRefreshToken(string refreshToken);

    Task<T?> GetUserAsync(int id);

    Task<T> CreateUserAsync(T agent);

    Task<bool> FindUserByEmailAsync(string Email);
}
