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
[Route("api/v1/public/medicos")]
public class PublicDoctorController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PublicDoctorController> _logger;

    public PublicDoctorController(ApplicationDbContext context, ILogger<PublicDoctorController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        try
        {
            var specializations = await _context.Doctors
                .Include(d => d.Specialization)
                .Include(d => d.DoctorMedicalCenters!)
                    .ThenInclude(dmc => dmc.MedicalCenter)
                        .ThenInclude(m => m.Address)
                .FirstOrDefaultAsync(d => d.Id == id);

            return StatusCode(200, ApiHelper.Ok(specializations!.ToViewModel()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }

    }
}