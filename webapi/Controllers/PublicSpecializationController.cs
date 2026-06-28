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
[Route("api/v1/public/especializacoes")]
public class PublicSpecializationController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PublicSpecializationController> _logger;

    public PublicSpecializationController(ApplicationDbContext context, ILogger<PublicSpecializationController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSpecializationAsync()
    {
        try
        {
            var specializations = await _context.Specializations
             .OrderByDescending(a => a.Id)
             .Select(s => new { s.Name, s.Id })
             .ToListAsync();

            return StatusCode(200, ApiHelper.Ok(specializations));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }

    }
}