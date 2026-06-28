using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using WebApi.Models;
using WebApi.Helpers;

namespace WebApi.Repositories;

public class UserRepository : IUserRepository<User>
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<UserRepository> _logger;

    public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .OrderByDescending(a => a.Id)
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User?> AddAsync(User user)
    {
        try
        {
            user.Password = PasswordHelper.HashPassword(user.Password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao adicionar item");
            throw;
        }
    }

    public async Task UpdateAsync(User user)
    {
        try
        {
            user.UpdatedAt = DateTime.UtcNow;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao artualizar item");
            throw;
        }
    }

    public async Task DeleteAsync(User user)
    {
        try
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<User?> GetByEmailAsync(string email, int? id = null)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email && (!id.HasValue || x.Id != id.Value));
    }
}