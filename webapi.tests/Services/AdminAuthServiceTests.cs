using System.Security.Claims;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Services;
using webapi.tests.Helpers;

namespace webapi.tests.Services;

public class AdminAuthServiceTests
{
    private static IConfiguration CreateMockJwtConfig()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "JwtSettings:AdminSecretKey", "MinhaChaveSuperSecretaParaAdminCom32Caracteres!" },
                { "JwtSettings:Issuer", "MediConnect" },
                { "JwtSettings:Audience", "MediConnectAdmin" }
            })
            .Build();
    }

    [Fact]
    public async Task ValidateUserAsync_ValidCredentials_ShouldReturnAdmin()
    {
        var context = TestDbContextFactory.CreateDbContext("AdminAuth_Validate_Valid");
        context.Administrators.Add(new Administrator { Id = 1, Name = "Admin Teste", Email = "admin@teste.com", Password = PasswordHelper.HashPassword("senha123") });
        context.SaveChanges();
        var service = new AdminAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<AdminAuthService>>());

        var result = await service.ValidateUserAsync("admin@teste.com", "senha123");

        result.Should().NotBeNull();
        result!.Name.Should().Be("Admin Teste");
    }

    [Fact]
    public async Task ValidateUserAsync_InvalidPassword_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("AdminAuth_Validate_InvalidPwd");
        context.Administrators.Add(new Administrator { Id = 1, Name = "Admin Teste", Email = "admin@teste.com", Password = PasswordHelper.HashPassword("senha123") });
        context.SaveChanges();
        var service = new AdminAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<AdminAuthService>>());

        var result = await service.ValidateUserAsync("admin@teste.com", "senha_errada");

        result.Should().BeNull();
    }

    [Fact]
    public async Task ValidateUserAsync_NonExistingEmail_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("AdminAuth_Validate_NotFound");
        var service = new AdminAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<AdminAuthService>>());

        var result = await service.ValidateUserAsync("naoexiste@teste.com", "senha123");

        result.Should().BeNull();
    }

    [Fact]
    public void CreateToken_ShouldGenerateValidJwt()
    {
        var context = TestDbContextFactory.CreateDbContext("AdminAuth_CreateToken");
        var service = new AdminAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<AdminAuthService>>());
        var admin = new Administrator { Id = 1, Name = "Admin", Email = "admin@teste.com" };

        var token = service.CreateToken(admin);

        token.Should().NotBeNullOrEmpty();
        token.Split('.').Should().HaveCount(3);
    }

    [Fact]
    public void CreateRefreshToken_ShouldGenerateValidJwt()
    {
        var context = TestDbContextFactory.CreateDbContext("AdminAuth_CreateRefresh");
        var service = new AdminAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<AdminAuthService>>());
        var admin = new Administrator { Id = 1, Name = "Admin", Email = "admin@teste.com" };

        var token = service.CreateRefreshToken(admin);

        token.Should().NotBeNullOrEmpty();
        token.Split('.').Should().HaveCount(3);
    }

    [Fact]
    public void ValidateRefreshToken_ValidToken_ShouldReturnClaimsPrincipal()
    {
        var context = TestDbContextFactory.CreateDbContext("AdminAuth_ValidateRefresh_Valid");
        var service = new AdminAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<AdminAuthService>>());
        var admin = new Administrator { Id = 1, Name = "Admin", Email = "admin@teste.com" };
        var refreshToken = service.CreateRefreshToken(admin);

        var principal = service.ValidateRefreshToken(refreshToken);

        principal.Should().NotBeNull();
        principal!.FindFirst(ClaimTypes.NameIdentifier)!.Value.Should().Be("1");
    }

    [Fact]
    public void ValidateRefreshToken_InvalidToken_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("AdminAuth_ValidateRefresh_Invalid");
        var service = new AdminAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<AdminAuthService>>());

        var principal = service.ValidateRefreshToken("invalid.token.here");

        principal.Should().BeNull();
    }

    [Fact]
    public async Task GetUserAsync_ExistingId_ShouldReturnAdmin()
    {
        var context = TestDbContextFactory.CreateDbContext("AdminAuth_GetUser_Exists");
        context.Administrators.Add(new Administrator { Id = 1, Name = "Admin", Email = "admin@teste.com", Password = "hash" });
        context.SaveChanges();
        var service = new AdminAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<AdminAuthService>>());

        var result = await service.GetUserAsync(1);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Admin");
    }

    [Fact]
    public async Task GetUserAsync_NonExistingId_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("AdminAuth_GetUser_NotFound");
        var service = new AdminAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<AdminAuthService>>());

        var result = await service.GetUserAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateUserAsync_ShouldAddAdmin()
    {
        var context = TestDbContextFactory.CreateDbContext("AdminAuth_CreateUser");
        var service = new AdminAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<AdminAuthService>>());
        var admin = new Administrator { Name = "Novo", Email = "novo@admin.com", Password = "hash" };

        var result = await service.CreateUserAsync(admin);

        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        context.Administrators.Count().Should().Be(1);
    }

    [Fact]
    public async Task FindUserByEmailAsync_ExistingEmail_ShouldReturnTrue()
    {
        var context = TestDbContextFactory.CreateDbContext("AdminAuth_FindByEmail_Exists");
        context.Administrators.Add(new Administrator { Id = 1, Name = "Admin", Email = "admin@teste.com", Password = "hash" });
        context.SaveChanges();
        var service = new AdminAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<AdminAuthService>>());

        var result = await service.FindUserByEmailAsync("admin@teste.com");

        result.Should().BeTrue();
    }

    [Fact]
    public async Task FindUserByEmailAsync_NonExistingEmail_ShouldReturnFalse()
    {
        var context = TestDbContextFactory.CreateDbContext("AdminAuth_FindByEmail_NotFound");
        var service = new AdminAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<AdminAuthService>>());

        var result = await service.FindUserByEmailAsync("naoexiste@teste.com");

        result.Should().BeFalse();
    }
}
