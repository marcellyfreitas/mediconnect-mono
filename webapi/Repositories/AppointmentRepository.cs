using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models.ViewModels;
using WebApi.Helpers;

namespace WebApi.Repositories;

public class AppointmentRepository : IRepository<Appointment>
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<AppointmentRepository> _logger;

    public AppointmentRepository(ApplicationDbContext context, ILogger<AppointmentRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Appointment>> GetAllAsync()
    {
        return await _context.Appointments
            .OrderByDescending(a => a.Id)
            .Include(r => r.Doctor)
            .Include(r => r.MedicalCenter).ThenInclude(m => m!.Address)
            .Include(r => r.User)
            .Include(r => r.AppointmentRating)
            .ToListAsync();
    }

    public async Task<Appointment?> GetByIdAsync(int id)
    {
        return await _context.Appointments
            .Include(r => r.Doctor).ThenInclude(d => d!.Specialization)
            .Include(r => r.MedicalCenter).ThenInclude(m => m!.Address)
            .Include(r => r.User)
            .Include(r => r.AppointmentRating)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Appointment?> AddAsync(Appointment appointment)
    {
        try
        {
            appointment.Protocol = ProtocolHelper.GenerateProtocol();
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return appointment;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao adicionar item");
            throw;
        }
    }

    public async Task UpdateAsync(Appointment appointment)
    {
        try
        {
            appointment.UpdatedAt = DateTime.UtcNow;
            _context.Entry(appointment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task DeleteAsync(Appointment appointment)
    {
        try
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}