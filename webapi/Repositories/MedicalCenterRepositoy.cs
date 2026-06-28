using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Repositories;

public class MedicalCenterRepository : IUserRepository<MedicalCenter>
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<MedicalCenterRepository> _logger;

    public MedicalCenterRepository(ApplicationDbContext context, ILogger<MedicalCenterRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<MedicalCenter>> GetAllAsync()
    {
        return await _context.MedicalCenters
            .OrderByDescending(a => a.Id)
            .Include(u => u.DoctorMedicalCenters!)
            .ThenInclude(dmc => dmc.Doctor)
            .Include(r => r.Address)
            .ToListAsync();
    }

    public async Task<MedicalCenter?> GetByIdAsync(int id)
    {
        return await _context.MedicalCenters
            .Include(u => u.DoctorMedicalCenters!)
            .ThenInclude(dmc => dmc.Doctor)
            .Include(r => r.Address)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<MedicalCenter?> AddAsync(MedicalCenter medicalCenter)
    {
        try
        {
            _context.MedicalCenters.Add(medicalCenter);
            await _context.SaveChangesAsync();

            return medicalCenter;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao adicionar item");
            throw;
        }
    }

    public async Task UpdateAsync(MedicalCenter medicalCenter)
    {
        try
        {
            medicalCenter.UpdatedAt = DateTime.UtcNow;
            _context.Entry(medicalCenter).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task DeleteAsync(MedicalCenter medicalCenter)
    {
        try
        {
            _context.MedicalCenters.Remove(medicalCenter);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<MedicalCenter?> GetByEmailAsync(string email, int? id = null)
    {
        return await _context.MedicalCenters
             .FirstOrDefaultAsync(x => x.Email == email && (!id.HasValue || x.Id != id.Value));
    }
}