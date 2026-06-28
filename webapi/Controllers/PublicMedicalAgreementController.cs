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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApi.Controllers;

[Authorize(Policy = "UserPolicy")]
[ApiController]
[Route("api/v1/public/convenios")]
public class PublicMedicalAgreementController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PublicMedicalAgreementController> _logger;

    public PublicMedicalAgreementController(ApplicationDbContext context, ILogger<PublicMedicalAgreementController> logger)
    {
        _context = context;
        _logger = logger;
    }


    [HttpGet("")]
    public async Task<ActionResult> GetAllAsync()
    {
        try
        {
            var convenios = await _context.MedicalAgreements
            .OrderByDescending(a => a.Id)
            .Select(m => m.ToViewModel())
            .ToListAsync();

            return StatusCode(200, ApiHelper.Ok(convenios));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}