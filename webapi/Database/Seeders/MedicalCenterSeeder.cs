using WebApi.Models;
using WebApi.Database;
using Bogus;
using System.Linq;

namespace WebApi.Database.Seeders;

public class MedicalCenterSeeder : ISeeder
{
    private readonly ApplicationDbContext _context;
    private readonly Faker _faker;

    public MedicalCenterSeeder(ApplicationDbContext context)
    {
        _context = context;
        _faker = new Faker("pt_BR");
    }

    public async Task Seed()
    {
        if (!_context.MedicalCenters.Any())
        {
            // Configuração do Faker para unidades médicas
            var medicalCenterFaker = new Faker<MedicalCenter>("pt_BR")
                .RuleFor(m => m.Name, f => GenerateMedicalCenterName(f))
                .RuleFor(m => m.PhoneNumber, f => GenerateBrazilianPhoneNumber(f))
                .RuleFor(m => m.Email, (f, m) => GenerateUniqueMedicalCenterEmail(f, m.Name))
                .RuleFor(m => m.AddressId, f => f.Random.Number(1, 10)); // Assumindo que existem endereços com IDs 1-10

            // Gerar 10 unidades médicas
            var medicalCenters = medicalCenterFaker.Generate(10);

            await _context.MedicalCenters.AddRangeAsync(medicalCenters);
            await _context.SaveChangesAsync();
        }
    }

    private string GenerateMedicalCenterName(Faker f)
    {
        var prefixes = new[] { "Unidade Médica", "Centro de Saúde", "Clínica", "Hospital", "Instituto", "Centro Médico" };
        var suffixes = new[] { "do Bem Estar", "Vida Plena", "São Lucas", "Santa Clara", "Popular", "Primavera", "Esperança", "Vida Nova", "Integral", "Andes" };

        return $"{f.PickRandom(prefixes)} {f.PickRandom(suffixes)}";
    }

    private string GenerateBrazilianPhoneNumber(Faker f)
    {
        var ddd = f.Random.Number(11, 99).ToString("00");
        var number = f.Random.Number(100000000, 999999999).ToString("00000-0000");
        return $"({ddd}) {number}";
    }

    private string GenerateUniqueMedicalCenterEmail(Faker f, string name)
    {
        var cleanName = name.Replace(" ", "").Replace(".", "").ToLower();
        return $"{cleanName}@unidadesmedicas.com";
    }
}