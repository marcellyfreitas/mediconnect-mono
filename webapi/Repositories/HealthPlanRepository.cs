using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Repositories;

public class HealthPlanRepository : IRepository<HealthPlan>
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<HealthPlanRepository> _logger;

    public HealthPlanRepository(ApplicationDbContext context, ILogger<HealthPlanRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<HealthPlan>> GetAllAsync()
    {
        return await _context.HealthPlans
            .OrderByDescending(a => a.Id)
            .Include(r => r.MedicalAgreements)
            .ToListAsync();
    }

    public async Task<HealthPlan?> GetByIdAsync(int id)
    {
        return await _context.HealthPlans
            .Include(r => r.MedicalAgreements)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<HealthPlan?> AddAsync(HealthPlan healthplan)
    {
        try
        {
            _context.HealthPlans.Add(healthplan);
            await _context.SaveChangesAsync();

            return healthplan;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao adicionar item");
            throw;
        }
    }

    public async Task UpdateAsync(HealthPlan healthplan)
    {
        try
        {
            healthplan.UpdatedAt = DateTime.UtcNow;
            _context.Entry(healthplan).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task DeleteAsync(HealthPlan healthplan)
    {
        try
        {
            _context.HealthPlans.Remove(healthplan);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}