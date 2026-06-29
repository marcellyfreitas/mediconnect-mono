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

public class UserAuthServiceTests
{
    private static IConfiguration CreateMockJwtConfig()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "JwtSettings:UserSecretKey", "MinhaChaveSuperSecretaParaUserCom32Caracteres!" },
                { "JwtSettings:Issuer", "MediConnect" },
                { "JwtSettings:Audience", "MediConnectUser" }
            })
            .Build();
    }

    [Fact]
    public async Task ValidateUserAsync_ValidCredentials_ShouldReturnUser()
    {
        var context = TestDbContextFactory.CreateDbContext("UserAuth_Validate_Valid");
        context.Users.Add(new User { Id = 1, Name = "User Teste", Email = "user@teste.com", Password = PasswordHelper.HashPassword("senha123"), Cpf = "12345678901" });
        context.SaveChanges();
        var service = new UserAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<UserAuthService>>());

        var result = await service.ValidateUserAsync("user@teste.com", "senha123");

        result.Should().NotBeNull();
        result!.Name.Should().Be("User Teste");
    }

    [Fact]
    public async Task ValidateUserAsync_InvalidPassword_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("UserAuth_Validate_InvalidPwd");
        context.Users.Add(new User { Id = 1, Name = "User Teste", Email = "user@teste.com", Password = PasswordHelper.HashPassword("senha123"), Cpf = "12345678901" });
        context.SaveChanges();
        var service = new UserAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<UserAuthService>>());

        var result = await service.ValidateUserAsync("user@teste.com", "senha_errada");

        result.Should().BeNull();
    }

    [Fact]
    public async Task ValidateUserAsync_NonExistingEmail_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("UserAuth_Validate_NotFound");
        var service = new UserAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<UserAuthService>>());

        var result = await service.ValidateUserAsync("naoexiste@teste.com", "senha123");

        result.Should().BeNull();
    }

    [Fact]
    public void CreateToken_ShouldGenerateValidJwt()
    {
        var context = TestDbContextFactory.CreateDbContext("UserAuth_CreateToken");
        var service = new UserAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<UserAuthService>>());
        var user = new User { Id = 1, Name = "User", Email = "user@teste.com" };

        var token = service.CreateToken(user);

        token.Should().NotBeNullOrEmpty();
        token.Split('.').Should().HaveCount(3);
    }

    [Fact]
    public void CreateRefreshToken_ShouldGenerateValidJwt()
    {
        var context = TestDbContextFactory.CreateDbContext("UserAuth_CreateRefresh");
        var service = new UserAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<UserAuthService>>());
        var user = new User { Id = 1, Name = "User", Email = "user@teste.com" };

        var token = service.CreateRefreshToken(user);

        token.Should().NotBeNullOrEmpty();
        token.Split('.').Should().HaveCount(3);
    }

    [Fact]
    public void ValidateRefreshToken_ValidToken_ShouldReturnClaimsPrincipal()
    {
        var context = TestDbContextFactory.CreateDbContext("UserAuth_ValidateRefresh_Valid");
        var service = new UserAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<UserAuthService>>());
        var user = new User { Id = 1, Name = "User", Email = "user@teste.com" };
        var refreshToken = service.CreateRefreshToken(user);

        var principal = service.ValidateRefreshToken(refreshToken);

        principal.Should().NotBeNull();
        principal!.FindFirst(ClaimTypes.NameIdentifier)!.Value.Should().Be("1");
    }

    [Fact]
    public void ValidateRefreshToken_InvalidToken_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("UserAuth_ValidateRefresh_Invalid");
        var service = new UserAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<UserAuthService>>());

        var principal = service.ValidateRefreshToken("invalid.token.here");

        principal.Should().BeNull();
    }

    [Fact]
    public async Task GetUserAsync_ExistingId_ShouldReturnUser()
    {
        var context = TestDbContextFactory.CreateDbContext("UserAuth_GetUser_Exists");
        context.Users.Add(new User { Id = 1, Name = "User", Email = "user@teste.com", Password = "hash", Cpf = "12345678901" });
        context.SaveChanges();
        var service = new UserAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<UserAuthService>>());

        var result = await service.GetUserAsync(1);

        result.Should().NotBeNull();
        result!.Name.Should().Be("User");
    }

    [Fact]
    public async Task GetUserAsync_NonExistingId_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("UserAuth_GetUser_NotFound");
        var service = new UserAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<UserAuthService>>());

        var result = await service.GetUserAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateUserAsync_ShouldAddUser()
    {
        var context = TestDbContextFactory.CreateDbContext("UserAuth_CreateUser");
        var service = new UserAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<UserAuthService>>());
        var user = new User { Name = "Novo", Email = "novo@user.com", Password = "hash123", Cpf = "12345678901" };

        var result = await service.CreateUserAsync(user);

        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        context.Users.Count().Should().Be(1);
    }

    [Fact]
    public async Task FindUserByEmailAsync_ExistingEmail_ShouldReturnTrue()
    {
        var context = TestDbContextFactory.CreateDbContext("UserAuth_FindByEmail_Exists");
        context.Users.Add(new User { Id = 1, Name = "User", Email = "user@teste.com", Password = "hash", Cpf = "12345678901" });
        context.SaveChanges();
        var service = new UserAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<UserAuthService>>());

        var result = await service.FindUserByEmailAsync("user@teste.com");

        result.Should().BeTrue();
    }

    [Fact]
    public async Task FindUserByEmailAsync_NonExistingEmail_ShouldReturnFalse()
    {
        var context = TestDbContextFactory.CreateDbContext("UserAuth_FindByEmail_NotFound");
        var service = new UserAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<UserAuthService>>());

        var result = await service.FindUserByEmailAsync("naoexiste@teste.com");

        result.Should().BeFalse();
    }

    [Fact]
    public async Task CreateAddressAndBindUser_ShouldCreateAddressAndBind()
    {
        var context = TestDbContextFactory.CreateDbContext("UserAuth_CreateAddress");
        context.Users.Add(new User { Id = 1, Name = "User", Email = "user@teste.com", Password = "hash", Cpf = "12345678901" });
        context.SaveChanges();
        var service = new UserAuthService(context, CreateMockJwtConfig(), Mock.Of<ILogger<UserAuthService>>());
        var address = new Address { Logradouro = "Rua Teste", Cep = "00000-000", Bairro = "Centro", Cidade = "SP", Estado = "SP" };

        var result = await service.CreateAddressAndBindUser(address, 1);

        result.Should().NotBeNull();
        result!.Id.Should().BeGreaterThan(0);
        var user = context.Users.Find(1);
        user!.AddressId.Should().Be(result.Id);
    }
}
