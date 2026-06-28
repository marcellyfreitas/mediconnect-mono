using WebApi.Models;
using WebApi.Database;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Database.Seeders;

public class MedicalAgreementSeeder : ISeeder
{
    private readonly ApplicationDbContext _context;

    public MedicalAgreementSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Seed()
    {
        if (!await _context.MedicalAgreements.AnyAsync())
        {
            var medicalAgreements = new List<MedicalAgreement>
            {
                new MedicalAgreement { Name = "Amil" },
                new MedicalAgreement { Name = "Bradesco Saúde" },
                new MedicalAgreement { Name = "Cassi" },
                new MedicalAgreement { Name = "IAPEP" },
                new MedicalAgreement { Name = "Intermédica" },
                new MedicalAgreement { Name = "Mediservice" },
                new MedicalAgreement { Name = "NotreDame Intermédica" },
                new MedicalAgreement { Name = "Porto Seguro Saúde" },
                new MedicalAgreement { Name = "São Cristóvão Saúde" },
                new MedicalAgreement { Name = "Unimed" },
            };

            foreach (var medicalAgreement in medicalAgreements)
            {
                await _context.MedicalAgreements.AddAsync(medicalAgreement);
            }

            await _context.SaveChangesAsync();
        }
    }

}
