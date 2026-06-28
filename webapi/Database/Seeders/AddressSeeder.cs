using WebApi.Models;
using WebApi.Database;
using Bogus;

namespace WebApi.Database.Seeders;

public class AddressSeeder : ISeeder
{
    private readonly ApplicationDbContext _context;

    public AddressSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Seed()
    {
        if (!_context.Addresses.Any())
        {
            var addressFaker = new Faker<Models.Address>("pt_BR")
                .RuleFor(a => a.Logradouro, f => f.Address.StreetName())
                .RuleFor(a => a.Cep, f => f.Address.ZipCode("#####-###"))
                .RuleFor(a => a.Bairro, f => f.Address.CitySuffix())
                .RuleFor(a => a.Cidade, f => f.Address.City())
                .RuleFor(a => a.Estado, f => f.Address.StateAbbr())
                .RuleFor(a => a.Pais, _ => "Brasil")
                .RuleFor(a => a.Numero, f => f.Random.Number(1, 3000).ToString())
                .RuleFor(a => a.Complemento, f =>
                    f.PickRandom(new[] {
                        "Apartamento " + f.Random.Number(101, 999),
                        "Casa",
                        "Sala " + f.Random.Number(1, 50),
                        "Bloco B",
                        "Fundos",
                        "Cobertura",
                        "Edifício " + f.Address.StreetName(),
                        "Chalé " + f.Random.Number(1, 10),
                        null
                    }));

            var addresses = addressFaker.Generate(50);

            await _context.Addresses.AddRangeAsync(addresses);
            await _context.SaveChangesAsync();
        }
    }
}