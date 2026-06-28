using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Repositories;

public class MedicalExamRepository : IRepository<MedicalExam>
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<MedicalExamRepository> _logger;

    public MedicalExamRepository(ApplicationDbContext context, ILogger<MedicalExamRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<MedicalExam>> GetAllAsync()
    {
        return await _context.MedicalExams
            .OrderByDescending(a => a.Id)
            .ToListAsync();
    }

    public async Task<MedicalExam?> GetByIdAsync(int id)
    {
        return await _context.MedicalExams.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<MedicalExam?> AddAsync(MedicalExam medicalExam)
    {
        try
        {
            _context.MedicalExams.Add(medicalExam);
            await _context.SaveChangesAsync();

            return medicalExam;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao adicionar item");
            throw;
        }
    }

    public async Task UpdateAsync(MedicalExam medicalExam)
    {
        try
        {
            medicalExam.UpdatedAt = DateTime.UtcNow;
            _context.Entry(medicalExam).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task DeleteAsync(MedicalExam medicalExam)
    {
        try
        {
            _context.MedicalExams.Remove(medicalExam);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}