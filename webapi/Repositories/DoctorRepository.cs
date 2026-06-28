using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Repositories;

public class DoctorRepository : IRepository<Doctor>
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<DoctorRepository> _logger;

    public DoctorRepository(ApplicationDbContext context, ILogger<DoctorRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Doctor>> GetAllAsync()
    {
        return await _context.Doctors
            .Include(u => u.Specialization)
            .Include(d => d.DoctorMedicalCenters!)
            .ThenInclude(dmc => dmc.MedicalCenter)
            .AsNoTracking()
            .OrderByDescending(a => a.Id)
            .ToListAsync();
    }

    public async Task<Doctor?> GetByIdAsync(int id)
    {
        return await _context.Doctors
            .Include(u => u.Specialization)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Doctor?> AddAsync(Doctor doctor)
    {
        try
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return doctor;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao adicionar item");
            throw;
        }
    }

    public async Task UpdateAsync(Doctor doctor)
    {
        try
        {
            doctor.UpdatedAt = DateTime.UtcNow;
            _context.Entry(doctor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task DeleteAsync(Doctor doctor)
    {
        try
        {
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}