using Microsoft.AspNetCore.Mvc;
using WebApi.Repositories;
using WebApi.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebApi.Models.ViewModels;
using WebApi.Models;
using WebApi.Helpers;

namespace WebApi.Controllers;

[Route("api/v1/auth/user")]
public class UserAuthController : ControllerBase
{
    private readonly IAuthenticationRepository<User> _repository;
    private readonly ILogger<UserAuthController> _logger;

    public UserAuthController(IAuthenticationRepository<User> authRepository, ILogger<UserAuthController> logger)
    {
        _repository = authRepository;
        _logger = logger;
    }

    protected UserViewModel GetViewModel(User user)
    {
        var viewModel = new UserViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Cpf = user.Cpf,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
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
            _logger.LogError(ex, ex.Message);
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

            var user = await _repository.GetUserAsync(UserId);

            if (user == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            var newAccessToken = _repository.CreateToken(user);
            var newRefreshToken = _repository.CreateRefreshToken(user);
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
    [Authorize(Policy = "UserPolicy")]
    public IActionResult Logout()
    {
        return StatusCode(200, ApiHelper.Ok(Array.Empty<int>(), "Logout efetuado com sucesso"));
    }

    [HttpGet("usuario")]
    [Authorize(Policy = "UserPolicy")]
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
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao carregar dados de usuário");
            throw;
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

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Cpf = dto.Cpf,
                Password = dto.Password,
            };

            await _repository.CreateUserAsync(user);

            var viewModel = GetViewModel(user);

            return StatusCode(201, ApiHelper.Created(viewModel));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

}