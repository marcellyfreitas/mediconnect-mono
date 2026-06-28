using WebApi.Models;
using WebApi.Database;
using Bogus;
using Bogus.DataSets;
using WebApi.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApi.Database.Seeders;

public class DoctorSeeder : ISeeder
{
    private readonly ApplicationDbContext _context;

    public DoctorSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Seed()
    {
        if (!await _context.Doctors.AnyAsync())
        {
            var specializations = await _context.Specializations
                .Select(s => s.Id)
                .ToListAsync();

            if (!specializations.Any())
            {
                throw new Exception("Nenhuma especialização encontrada.");
            }

            Random r = new Random();
            var usedEmails = new HashSet<string>();
            var doctorFaker = new Faker<Doctor>("pt_BR")
                .RuleFor(d => d.Name, f => f.Name.FullName())
                .RuleFor(d => d.CPF, _ => CpfHelper.Generate())
                .RuleFor(d => d.Email, (f, d) =>
                {
                    string email;
                    do
                    {
                        email = f.Internet.Email(d.Name.ToLower());
                    } while (!usedEmails.Add(email));
                    return email;
                })
                .RuleFor(d => d.CRM, f => $"{f.Random.Number(100000, 999999)}/{f.Address.StateAbbr()}")
                .RuleFor(d => d.SpecializationId, f => r.Next(1, 12));

            var doctors = doctorFaker.Generate(100);

            await _context.Doctors.AddRangeAsync(doctors);
            await _context.SaveChangesAsync();
        }
    }
}
