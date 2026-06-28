using WebApi.Helpers;
using WebApi.Models;
using WebApi.Database;
using Bogus;

namespace WebApi.Database.Seeders;

public class AdministratorSeeder : ISeeder
{
    private readonly ApplicationDbContext _context;

    public AdministratorSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Seed()
    {
        if (!_context.Administrators.Any(a => a.Email == "administrador@administrador.com"))
        {
            var adminFaker = new Faker<Administrator>()
                .RuleFor(a => a.Name, f => "Administrador")
                .RuleFor(a => a.Email, f => "administrador@administrador.com")
                .RuleFor(a => a.Password, f => PasswordHelper.HashPassword("Senh@123"))
                .RuleFor(a => a.CreatedAt, f => DateTime.UtcNow);

            if (!_context.Administrators.Any(a => a.Email == "administrador@administrador.com"))
            {
                var administrator = adminFaker.Generate();

                await _context.Administrators.AddAsync(administrator);
                await _context.SaveChangesAsync();
            }

            await GenerateAdditionalAdminsAsync(5);
        }
    }

    private async Task GenerateAdditionalAdminsAsync(int count)
    {
        var usedEmails = new HashSet<string>(_context.Administrators.Select(a => a.Email));

        var additionalAdminFaker = new Faker<Administrator>("pt_BR")
            .RuleFor(a => a.Name, f => f.Name.FullName())
            .RuleFor(a => a.Email, f =>
            {
                string email;
                do
                {
                    email = f.Internet.Email(f.Person.FirstName.ToLower());
                } while (usedEmails.Contains(email));

                usedEmails.Add(email);
                return email;
            })
            .RuleFor(a => a.Password, f => PasswordHelper.HashPassword(f.Internet.Password(10)))
            .RuleFor(a => a.CreatedAt, f => DateTime.UtcNow);

        var newAdmins = additionalAdminFaker.Generate(count);
        await _context.Administrators.AddRangeAsync(newAdmins);
        await _context.SaveChangesAsync();
    }
}