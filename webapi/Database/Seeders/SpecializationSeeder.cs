using WebApi.Models;
using WebApi.Database;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Database.Seeders;

public class SpecializationSeeder : ISeeder
{
    private readonly ApplicationDbContext _context;

    public SpecializationSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Seed()
    {
        if (!await _context.Specializations.AnyAsync())
        {
            var especializations = new List<Specialization>
            {
                new Specialization { Name = "Clínica Médica" },
                new Specialization { Name = "Cardiologia" },
                new Specialization { Name = "Dermatologia" },
                new Specialization { Name = "Endocrinologia" },
                new Specialization { Name = "Gastroenterologia" },
                new Specialization { Name = "Ginecologia e Obstetrícia" },
                new Specialization { Name = "Oftalmologia" },
                new Specialization { Name = "Ortopedia e Traumatologia" },
                new Specialization { Name = "Otorrinolaringologia" },
                new Specialization { Name = "Pediatria" },
                new Specialization { Name = "Psiquiatria" },
                new Specialization { Name = "Urologia" }
            };

            foreach (var specialization in especializations)
            {
                await _context.Specializations.AddAsync(specialization);
            }

            await _context.SaveChangesAsync();
        }
    }

}
