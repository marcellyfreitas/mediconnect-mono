using Microsoft.AspNetCore.Mvc;
using WebApi.Repositories;
using WebApi.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebApi.Models.ViewModels;
using WebApi.Models;
using WebApi.Helpers;

namespace WebApi.Controllers;

[Route("api/v1/auth/admin")]
public class AdminAuthController : ControllerBase
{
    private readonly IAuthenticationRepository<Administrator> _repository;
    private readonly ILogger<AdminAuthController> _logger;

    public AdminAuthController(IAuthenticationRepository<Administrator> authRepository, ILogger<AdminAuthController> logger)
    {
        _repository = authRepository;
        _logger = logger;
    }

    protected AdministratorViewModel GetViewModel(Administrator administrator)
    {
        var viewModel = new AdministratorViewModel
        {
            Id = administrator.Id,
            Name = administrator.Name,
            Email = administrator.Email,
            CreatedAt = administrator.CreatedAt,
            UpdatedAt = administrator.UpdatedAt,
        };

        return viewModel;
    }

    [HttpPost("login")]
    public async Task<IActionResult> AuthenticateUser([FromBody] LoginDto dto)
    {
        try
        {
            ModelState.ClearValidationState(nameof(dto));

            if (!TryValidateModel(dto))
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));
            }

            var model = await _repository.ValidateUserAsync(dto.Email, dto.Password);

            if (model == null)
            {
                return StatusCode(401, ApiHelper.Unauthorized());
            }

            var accessToken = _repository.CreateToken(model);
            var refreshToken = _repository.CreateRefreshToken(model);
            var tokens = new { AccessToken = accessToken, RefreshToken = refreshToken };

            return StatusCode(200, ApiHelper.Ok(tokens));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao tentar autenticar");
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
    {
        try
        {
            ModelState.ClearValidationState(nameof(dto));

            if (!TryValidateModel(dto))
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));
            }

            var principal = _repository.ValidateRefreshToken(dto.RefreshToken);

            if (principal == null)
            {
                return StatusCode(401, ApiHelper.Unauthorized("Refresh token inválido ou expirado"));
            }

            var UserIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (UserIdClaim == null || !int.TryParse(UserIdClaim, out int UserId))
            {
                return StatusCode(401, ApiHelper.Unauthorized("Token inválido"));
            }

            var administrator = await _repository.GetUserAsync(UserId);

            if (administrator == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            var newAccessToken = _repository.CreateToken(administrator);
            var newRefreshToken = _repository.CreateRefreshToken(administrator);
            var tokens = new { AccessToken = newAccessToken, RefreshToken = newRefreshToken };

            return StatusCode(200, ApiHelper.Ok(tokens));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPost("logout")]
    [Authorize(Policy = "AdminPolicy")]
    public IActionResult Logout()
    {
        return Ok(new { Message = "Logout realizado com sucesso" });
    }

    [HttpGet("usuario")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetUserData()
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return StatusCode(401, ApiHelper.Unauthorized("Token inválido"));
            }

            var user = await _repository.GetUserAsync(userId);

            if (user == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            var viewModel = GetViewModel(user);


            return StatusCode(200, ApiHelper.Ok(viewModel));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        try
        {
            ModelState.ClearValidationState(nameof(dto));

            if (!TryValidateModel(dto))
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));
            }

            var existingUser = await _repository.FindUserByEmailAsync(dto.Email);

            if (existingUser)
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(Array.Empty<int>(), "Já existe um usuário com este email"));
            }

            var user = new Administrator
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = PasswordHelper.HashPassword(dto.Password),
            };

            await _repository.CreateUserAsync(user);

            var result = await GetUserData();

            if (result is ObjectResult objectResult)
            {
                objectResult.StatusCode = 201;
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

}