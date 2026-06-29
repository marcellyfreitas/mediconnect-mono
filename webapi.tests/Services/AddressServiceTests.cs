using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Models;
using WebApi.Services;
using webapi.tests.Helpers;

namespace webapi.tests.Services;

public class AddressServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAddressesOrderedByIdDescending()
    {
        var context = TestDbContextFactory.CreateDbContext("Address_GetAll");
        TestDbContextFactory.SeedDbContext(context);
        var logger = Mock.Of<ILogger<AddressService>>();
        var service = new AddressService(context, logger);

        var result = await service.GetAllAsync();

        result.Should().HaveCount(2);
        result.First().Id.Should().Be(2);
        result.Last().Id.Should().Be(1);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ShouldReturnAddress()
    {
        var context = TestDbContextFactory.CreateDbContext("Address_GetById_Exists");
        TestDbContextFactory.SeedDbContext(context);
        var service = new AddressService(context, Mock.Of<ILogger<AddressService>>());

        var result = await service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Logradouro.Should().Be("Rua A");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("Address_GetById_NotFound");
        TestDbContextFactory.SeedDbContext(context);
        var service = new AddressService(context, Mock.Of<ILogger<AddressService>>());

        var result = await service.GetByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldAddAddressAndReturnIt()
    {
        var context = TestDbContextFactory.CreateDbContext("Address_Add");
        var service = new AddressService(context, Mock.Of<ILogger<AddressService>>());
        var address = new Address { Logradouro = "Rua Nova", Cep = "00000-000", Bairro = "Novo", Cidade = "SP", Estado = "SP" };

        var result = await service.AddAsync(address);

        result.Should().NotBeNull();
        result!.Id.Should().BeGreaterThan(0);
        context.Addresses.Count().Should().Be(1);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAddressAndSetUpdatedAt()
    {
        var context = TestDbContextFactory.CreateDbContext("Address_Update");
        TestDbContextFactory.SeedDbContext(context);
        var service = new AddressService(context, Mock.Of<ILogger<AddressService>>());
        var address = await service.GetByIdAsync(1);
        address!.Logradouro = "Rua Alterada";

        await service.UpdateAsync(address);

        var updated = await service.GetByIdAsync(1);
        updated!.Logradouro.Should().Be("Rua Alterada");
        updated.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveAddress()
    {
        var context = TestDbContextFactory.CreateDbContext("Address_Delete");
        TestDbContextFactory.SeedDbContext(context);
        var service = new AddressService(context, Mock.Of<ILogger<AddressService>>());
        var address = await service.GetByIdAsync(1);

        await service.DeleteAsync(address!);

        context.Addresses.Count().Should().Be(1);
        (await service.GetByIdAsync(1)).Should().BeNull();
    }
}
