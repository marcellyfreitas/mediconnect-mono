using WebApi.Helpers;
using WebApi.Models;
using WebApi.Database;
using Bogus;
using System.Linq;

namespace WebApi.Database.Seeders;

public class UserSeeder : ISeeder
{
    private readonly ApplicationDbContext _context;
    public UserSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Seed()
    {
        if (!_context.Users.Any(a => a.Email == "usuario@usuario.com"))
        {
            var userFaker = new Faker<User>()
                .RuleFor(a => a.Name, f => "Jon Snow")
                .RuleFor(a => a.Email, f => "usuario@usuario.com")
                .RuleFor(a => a.Cpf, f => CpfHelper.Generate())
                .RuleFor(a => a.Password, f => PasswordHelper.HashPassword("Senh@123"))
                .RuleFor(a => a.CreatedAt, f => DateTime.UtcNow);

            if (!_context.Users.Any(a => a.Email == "usuario@usuario.com"))
            {
                var user = userFaker.Generate();

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }

            await SeedFakeUsers(10);
        }
    }

    private async Task SeedFakeUsers(int quantity)
    {
        var existingEmails = _context.Users.Select(u => u.Email).ToHashSet();

        var userFaker = new Faker<User>("pt_BR")
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.Email, (f, u) =>
            {
                string email;
                do
                {
                    email = f.Internet.Email(u.Name.ToLower());
                } while (existingEmails.Contains(email));

                existingEmails.Add(email);
                return email;
            })
            .RuleFor(u => u.Password, f => PasswordHelper.HashPassword("Senh@123"))
            .RuleFor(u => u.Cpf, f => CpfHelper.Generate());

        var fakeUsers = userFaker.Generate(quantity);
        await _context.Users.AddRangeAsync(fakeUsers);
        await _context.SaveChangesAsync();
    }
}