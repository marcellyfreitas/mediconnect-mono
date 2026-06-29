using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Services;

public class AppointmentRatingService : IService<AppointmentRating>
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<AppointmentRatingService> _logger;

    public AppointmentRatingService(ApplicationDbContext context, ILogger<AppointmentRatingService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<AppointmentRating>> GetAllAsync()
    {
        return await _context.AppointmentRatings
            .OrderByDescending(a => a.Id)
            .Include(r => r.Appointment)
            .ToListAsync();
    }

    public async Task<AppointmentRating?> GetByIdAsync(int id)
    {
        return await _context.AppointmentRatings
            .Include(r => r.Appointment)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<AppointmentRating?> AddAsync(AppointmentRating appointmentRating)
    {
        try
        {
            _context.AppointmentRatings.Add(appointmentRating);
            await _context.SaveChangesAsync();

            return appointmentRating;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao adicionar item");
            throw;
        }
    }

    public async Task UpdateAsync(AppointmentRating appointmentRating)
    {
        try
        {
            appointmentRating.UpdatedAt = DateTime.UtcNow;
            _context.Entry(appointmentRating).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task DeleteAsync(AppointmentRating appointmentRating)
    {
        try
        {
            _context.AppointmentRatings.Remove(appointmentRating);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}