using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Models.Dto;
using WebApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using System.Security.Claims;
using WebApi.Helpers;
using WebApi.Extensions.ModelExtensions;

namespace WebApi.Controllers;

[Authorize(Policy = "UserPolicy")]
[ApiController]
[Route("api/v1/public/usuario")]
public class PublicUsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PublicUsersController> _logger;

    public PublicUsersController(ApplicationDbContext context, ILogger<PublicUsersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    protected async Task<User?> GetAuthenticatedUserAsync()
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return null;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpPut("")]
    public async Task<ActionResult> AlterUserData(UpdateUserDto dto)
    {
        try
        {
            ModelState.ClearValidationState(nameof(dto));

            if (!TryValidateModel(dto))
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));
            }

            var user = await GetAuthenticatedUserAsync();

            user!.Name = dto.Name ?? user.Name;
            user!.Email = dto.Email ?? user.Email;
            user!.Cpf = dto.Cpf ?? user.Cpf;
            user!.UpdatedAt = DateTime.UtcNow;

            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpDelete("")]
    public async Task<ActionResult> DeleteAccountAsync()
    {
        try
        {
            var user = await GetAuthenticatedUserAsync();

            if (user == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }
}