using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models.ViewModels;

namespace WebApi.Repositories;

public class AddressRepository : IRepository<Address>
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<AddressRepository> _logger;

    public AddressRepository(ApplicationDbContext context, ILogger<AddressRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Address>> GetAllAsync()
    {
        return await _context.Addresses
            .OrderByDescending(a => a.Id)
            .ToListAsync();
    }

    public async Task<Address?> GetByIdAsync(int id)
    {
        return await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Address?> AddAsync(Address address)
    {
        try
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return address;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao adicionar item");
            throw;
        }
    }

    public async Task UpdateAsync(Address address)
    {
        try
        {
            address.UpdatedAt = DateTime.UtcNow;
            _context.Entry(address).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task DeleteAsync(Address address)
    {
        try
        {
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}