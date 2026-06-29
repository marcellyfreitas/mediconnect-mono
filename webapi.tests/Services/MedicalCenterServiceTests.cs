using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Models;
using WebApi.Services;
using webapi.tests.Helpers;

namespace webapi.tests.Services;

public class MedicalCenterServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldIncludeDoctorsAndAddress()
    {
        var context = TestDbContextFactory.CreateDbContext("MedCenter_GetAll");
        TestDbContextFactory.SeedDbContext(context);
        var service = new MedicalCenterService(context, Mock.Of<ILogger<MedicalCenterService>>());

        var result = await service.GetAllAsync();

        result.Should().HaveCount(2);
        result.First().Address.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ShouldReturnWithIncludes()
    {
        var context = TestDbContextFactory.CreateDbContext("MedCenter_GetById_Exists");
        TestDbContextFactory.SeedDbContext(context);
        var service = new MedicalCenterService(context, Mock.Of<ILogger<MedicalCenterService>>());

        var result = await service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Hospital Central");
        result.Address.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("MedCenter_GetById_NotFound");
        TestDbContextFactory.SeedDbContext(context);
        var service = new MedicalCenterService(context, Mock.Of<ILogger<MedicalCenterService>>());

        var result = await service.GetByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldAdd()
    {
        var context = TestDbContextFactory.CreateDbContext("MedCenter_Add");
        var service = new MedicalCenterService(context, Mock.Of<ILogger<MedicalCenterService>>());

        var result = await service.AddAsync(new MedicalCenter { Name = "Nova Unidade", Email = "nu@email.com" });

        result.Should().NotBeNull();
        result!.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdate()
    {
        var context = TestDbContextFactory.CreateDbContext("MedCenter_Update");
        TestDbContextFactory.SeedDbContext(context);
        var service = new MedicalCenterService(context, Mock.Of<ILogger<MedicalCenterService>>());
        var mc = await service.GetByIdAsync(1);
        mc!.Name = "Hospital Central Updated";

        await service.UpdateAsync(mc);

        var updated = await service.GetByIdAsync(1);
        updated!.Name.Should().Be("Hospital Central Updated");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemove()
    {
        var context = TestDbContextFactory.CreateDbContext("MedCenter_Delete");
        TestDbContextFactory.SeedDbContext(context);
        var service = new MedicalCenterService(context, Mock.Of<ILogger<MedicalCenterService>>());

        await service.DeleteAsync((await service.GetByIdAsync(1))!);

        context.MedicalCenters.Count().Should().Be(1);
    }

    [Fact]
    public async Task GetByEmailAsync_ExistingEmail_ShouldReturn()
    {
        var context = TestDbContextFactory.CreateDbContext("MedCenter_GetByEmail_Exists");
        TestDbContextFactory.SeedDbContext(context);
        var service = new MedicalCenterService(context, Mock.Of<ILogger<MedicalCenterService>>());

        var result = await service.GetByEmailAsync("hc@email.com");

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByEmailAsync_NonExisting_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("MedCenter_GetByEmail_NotFound");
        TestDbContextFactory.SeedDbContext(context);
        var service = new MedicalCenterService(context, Mock.Of<ILogger<MedicalCenterService>>());

        var result = await service.GetByEmailAsync("inexistente@email.com");

        result.Should().BeNull();
    }
}
