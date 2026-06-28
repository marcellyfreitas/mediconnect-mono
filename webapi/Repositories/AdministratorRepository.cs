using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using WebApi.Models;
using WebApi.Database;
using WebApi.Helpers;

namespace WebApi.Repositories;

public class AdministratorRepository : IUserRepository<Administrator>
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<AdministratorRepository> _logger;

    public AdministratorRepository(ApplicationDbContext dBContext, ILogger<AdministratorRepository> logger)
    {
        _context = dBContext;
        _logger = logger;
    }

    public async Task<IEnumerable<Administrator>> GetAllAsync()
    {
        return await _context.Administrators
            .Where(u => u.Email != "administrador@administrador.com")
            .OrderByDescending(a => a.Id)
            .ToListAsync();
    }

    public async Task<Administrator?> GetByIdAsync(int id)
    {
        return await _context.Administrators.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Administrator?> AddAsync(Administrator administrator)
    {
        try
        {
            administrator.Password = PasswordHelper.HashPassword(administrator.Password);
            await _context.Administrators.AddAsync(administrator);
            await _context.SaveChangesAsync();
            return administrator;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao adicionar item");
            throw new Exception("Falha ao cadstrar usuario");
        }
    }

    public async Task UpdateAsync(Administrator administrator)
    {
        try
        {
            administrator.UpdatedAt = DateTime.UtcNow;
            _context.Entry(administrator).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }

    }

    public async Task DeleteAsync(Administrator administrator)
    {
        try
        {
            _context.Administrators.Remove(administrator);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }

    }

    public async Task<Administrator?> GetByEmailAsync(string email, int? id = null)
    {
        var models = _context.Administrators.AsQueryable();

        if (id != null)
        {
            models = models.Where(m => m.Id != id);
        }

        models = models.Where(m => m.Email == email);

        return await models.FirstOrDefaultAsync();
    }
}