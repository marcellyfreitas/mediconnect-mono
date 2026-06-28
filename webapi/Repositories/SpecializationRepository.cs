using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Repositories;

public class SpecializationRepository : IRepository<Specialization>
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<SpecializationRepository> _logger;

    public SpecializationRepository(ApplicationDbContext context, ILogger<SpecializationRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Specialization>> GetAllAsync()
    {
        return await _context.Specializations
            .OrderByDescending(a => a.Id)
            .ToListAsync();
    }

    public async Task<Specialization?> GetByIdAsync(int id)
    {
        return await _context.Specializations.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Specialization?> AddAsync(Specialization Specialization)
    {
        try
        {
            _context.Specializations.Add(Specialization);
            await _context.SaveChangesAsync();

            return Specialization;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao adicionar item");
            throw;
        }
    }

    public async Task UpdateAsync(Specialization Specialization)
    {
        try
        {
            Specialization.UpdatedAt = DateTime.UtcNow;
            _context.Entry(Specialization).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task DeleteAsync(Specialization Specialization)
    {
        try
        {
            _context.Specializations.Remove(Specialization);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}