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
[Route("api/v1/public/unidades")]
public class PublicMedicalCenterController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PublicMedicalCenterController> _logger;

    public PublicMedicalCenterController(ApplicationDbContext context, ILogger<PublicMedicalCenterController> logger)
    {
        _context = context;
        _logger = logger;
    }


    [HttpGet]
    public async Task<ActionResult> GetAllAsync()
    {
        try
        {
            var unidades = await _context.MedicalCenters
                .Include(m => m.Address) // Primeiro o Include
                .OrderByDescending(a => a.Id)
                .ToListAsync(); // Executa a query no banco

            // Depois faz o Select manualmente em memória
            var viewModels = unidades.Select(u => u.ToViewModel()).ToList();

            return StatusCode(200, ApiHelper.Ok(viewModels));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }


    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var unidades = await _context.MedicalCenters
                .Include(m => m.Address)
                .FirstOrDefaultAsync(u => u.Id == id);

            return StatusCode(200, ApiHelper.Ok(unidades!.ToViewModel()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}