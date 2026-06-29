using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Services;
using webapi.tests.Helpers;

namespace webapi.tests.Services;

public class UserServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        var context = TestDbContextFactory.CreateDbContext("User_GetAll");
        TestDbContextFactory.SeedDbContext(context);
        var service = new UserService(context, Mock.Of<ILogger<UserService>>());

        var result = await service.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ShouldReturnUser()
    {
        var context = TestDbContextFactory.CreateDbContext("User_GetById_Exists");
        TestDbContextFactory.SeedDbContext(context);
        var service = new UserService(context, Mock.Of<ILogger<UserService>>());

        var result = await service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Carlos");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("User_GetById_NotFound");
        TestDbContextFactory.SeedDbContext(context);
        var service = new UserService(context, Mock.Of<ILogger<UserService>>());

        var result = await service.GetByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldHashPasswordAndAddUser()
    {
        var context = TestDbContextFactory.CreateDbContext("User_Add");
        var service = new UserService(context, Mock.Of<ILogger<UserService>>());
        var user = new User { Name = "Novo", Email = "novo@email.com", Password = "minhaSenha123", Cpf = "12345678901" };

        var result = await service.AddAsync(user);

        result.Should().NotBeNull();
        result!.Password.Should().NotBe("minhaSenha123");
        PasswordHelper.VerifyPassword("minhaSenha123", result.Password).Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUserAndSetUpdatedAt()
    {
        var context = TestDbContextFactory.CreateDbContext("User_Update");
        TestDbContextFactory.SeedDbContext(context);
        var service = new UserService(context, Mock.Of<ILogger<UserService>>());
        var user = await service.GetByIdAsync(1);
        user!.Name = "Carlos Updated";

        await service.UpdateAsync(user);

        var updated = await service.GetByIdAsync(1);
        updated!.Name.Should().Be("Carlos Updated");
        updated.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveUser()
    {
        var context = TestDbContextFactory.CreateDbContext("User_Delete");
        TestDbContextFactory.SeedDbContext(context);
        var service = new UserService(context, Mock.Of<ILogger<UserService>>());
        var user = await service.GetByIdAsync(1);

        await service.DeleteAsync(user!);

        context.Users.Count().Should().Be(1);
    }

    [Fact]
    public async Task GetByEmailAsync_ExistingEmail_ShouldReturnUser()
    {
        var context = TestDbContextFactory.CreateDbContext("User_GetByEmail_Exists");
        TestDbContextFactory.SeedDbContext(context);
        var service = new UserService(context, Mock.Of<ILogger<UserService>>());

        var result = await service.GetByEmailAsync("carlos@email.com");

        result.Should().NotBeNull();
        result!.Name.Should().Be("Carlos");
    }

    [Fact]
    public async Task GetByEmailAsync_NonExistingEmail_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("User_GetByEmail_NotFound");
        TestDbContextFactory.SeedDbContext(context);
        var service = new UserService(context, Mock.Of<ILogger<UserService>>());

        var result = await service.GetByEmailAsync("inexistente@email.com");

        result.Should().BeNull();
    }
}
