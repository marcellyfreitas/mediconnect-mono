using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Services;
using webapi.tests.Helpers;

namespace webapi.tests.Services;

public class AdministratorServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldExcludeSeedAdmin()
    {
        var context = TestDbContextFactory.CreateDbContext("Admin_GetAll");
        TestDbContextFactory.SeedDbContext(context);
        context.Administrators.Add(new Administrator { Id = 3, Name = "Seed", Email = "administrador@administrador.com", Password = "hash" });
        context.SaveChanges();
        var service = new AdministratorService(context, Mock.Of<ILogger<AdministratorService>>());

        var result = await service.GetAllAsync();

        result.Should().HaveCount(2);
        result.Any(a => a.Email == "administrador@administrador.com").Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ShouldReturnAdministrator()
    {
        var context = TestDbContextFactory.CreateDbContext("Admin_GetById_Exists");
        TestDbContextFactory.SeedDbContext(context);
        var service = new AdministratorService(context, Mock.Of<ILogger<AdministratorService>>());

        var result = await service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Admin Principal");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("Admin_GetById_NotFound");
        TestDbContextFactory.SeedDbContext(context);
        var service = new AdministratorService(context, Mock.Of<ILogger<AdministratorService>>());

        var result = await service.GetByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldHashPassword()
    {
        var context = TestDbContextFactory.CreateDbContext("Admin_Add");
        var service = new AdministratorService(context, Mock.Of<ILogger<AdministratorService>>());
        var admin = new Administrator { Name = "Novo Admin", Email = "novo@admin.com", Password = "senha123" };

        var result = await service.AddAsync(admin);

        result.Should().NotBeNull();
        result!.Password.Should().NotBe("senha123");
        PasswordHelper.VerifyPassword("senha123", result.Password).Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAndSetUpdatedAt()
    {
        var context = TestDbContextFactory.CreateDbContext("Admin_Update");
        TestDbContextFactory.SeedDbContext(context);
        var service = new AdministratorService(context, Mock.Of<ILogger<AdministratorService>>());
        var admin = await service.GetByIdAsync(1);
        admin!.Name = "Admin Alterado";

        await service.UpdateAsync(admin);

        var updated = await service.GetByIdAsync(1);
        updated!.Name.Should().Be("Admin Alterado");
        updated.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveAdministrator()
    {
        var context = TestDbContextFactory.CreateDbContext("Admin_Delete");
        TestDbContextFactory.SeedDbContext(context);
        var service = new AdministratorService(context, Mock.Of<ILogger<AdministratorService>>());

        await service.DeleteAsync((await service.GetByIdAsync(1))!);

        context.Administrators.Count().Should().Be(1);
    }

    [Fact]
    public async Task GetByEmailAsync_ExistingEmail_ShouldReturnAdmin()
    {
        var context = TestDbContextFactory.CreateDbContext("Admin_GetByEmail_Exists");
        TestDbContextFactory.SeedDbContext(context);
        var service = new AdministratorService(context, Mock.Of<ILogger<AdministratorService>>());

        var result = await service.GetByEmailAsync("admin@admin.com");

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByEmailAsync_NonExistingEmail_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("Admin_GetByEmail_NotFound");
        TestDbContextFactory.SeedDbContext(context);
        var service = new AdministratorService(context, Mock.Of<ILogger<AdministratorService>>());

        var result = await service.GetByEmailAsync("inexistente@admin.com");

        result.Should().BeNull();
    }
}
