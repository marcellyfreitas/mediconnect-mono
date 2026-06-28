using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Database;

namespace WebApi.Repositories;

public class AdminAuthRepository : IAuthenticationRepository<Administrator>
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AdminAuthRepository> _logger;

    public AdminAuthRepository(
        ApplicationDbContext context,
        IConfiguration configuration,
        ILogger<AdminAuthRepository> logger
    )
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<Administrator?> ValidateUserAsync(string email, string password)
    {
        var user = await _context.Administrators.FirstOrDefaultAsync(x => x.Email == email);

        if (user == null || !PasswordHelper.VerifyPassword(password, user.Password))
        {
            return null;
        }

        return user;
    }

    public string CreateToken(Administrator administrador)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var privateKey = Encoding.UTF8.GetBytes(jwtSettings["AdminSecretKey"] ?? "");
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(privateKey),
                SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(1),
                Subject = GenerateClaims(administrador)
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Erro ao criar o token: parâmetro nulo");
            throw new Exception("Erro ao criar o token: parâmetro nulo", ex);
        }
        catch (FormatException ex)
        {
            _logger.LogError(ex, "Erro de formato ao criar o token");
            throw new Exception("Erro de formato ao criar o token", ex);
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogError(ex, "Erro ao assinar o token JWT");
            throw new Exception("Erro ao assinar o token JWT", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao criar o token");
            throw new Exception("Erro inesperado ao criar o token", ex);
        }
    }

    public string CreateRefreshToken(Administrator administrator)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var privateKey = Encoding.UTF8.GetBytes(jwtSettings["AdminSecretKey"] ?? "");
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(privateKey),
                SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddDays(7),
                Subject = GenerateClaims(administrator)
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar o refresh token");
            throw new Exception("Erro ao criar o refresh token", ex);
        }
    }

    public ClaimsPrincipal? ValidateRefreshToken(string refreshToken)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var privateKey = Encoding.UTF8.GetBytes(jwtSettings["AdminSecretKey"] ?? "");

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(privateKey)
            };

            var principal = handler.ValidateToken(refreshToken, validationParameters, out _);
            return principal;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao validar o refresh token");
            return null;
        }
    }

    private ClaimsIdentity GenerateClaims(Administrator administrator)
    {
        try
        {
            var ci = new ClaimsIdentity();

            ci.AddClaim(new Claim(ClaimTypes.NameIdentifier, administrator.Id.ToString()));
            ci.AddClaim(new Claim(ClaimTypes.Name, administrator.Name));
            ci.AddClaim(new Claim(ClaimTypes.GivenName, administrator.Name));
            ci.AddClaim(new Claim(ClaimTypes.Email, administrator.Email));

            return ci;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao criar o token");
            throw;
        }
    }

    public async Task<Administrator?> GetUserAsync(int id)
    {
        try
        {
            var administrator = await _context.Administrators
            .FirstOrDefaultAsync(x => x.Id == id);

            if (administrator == null)
            {
                return null;
            }

            return administrator;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao capturar dados de usuário");
            throw;
        }

    }

    public async Task<Administrator> CreateUserAsync(Administrator administrator)
    {
        try
        {
            _context.Administrators.Add(administrator);

            await _context.SaveChangesAsync();

            return administrator;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao criar usuário");
            throw;
        }
    }

    public async Task<bool> FindUserByEmailAsync(string Email)
    {
        try
        {
            var administrator = await _context.Administrators.FirstOrDefaultAsync(u => u.Email == Email);

            if (administrator == null)
            {
                return false;
            }

            return true;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao capturar usuário eplo email");
            throw;
        }
    }
}