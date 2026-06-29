using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Models;
using WebApi.Services;
using webapi.tests.Helpers;

namespace webapi.tests.Services;

public class SpecializationServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnAll()
    {
        var context = TestDbContextFactory.CreateDbContext("Spec_GetAll");
        TestDbContextFactory.SeedDbContext(context);
        var service = new SpecializationService(context, Mock.Of<ILogger<SpecializationService>>());

        var result = await service.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ShouldReturn()
    {
        var context = TestDbContextFactory.CreateDbContext("Spec_GetById_Exists");
        TestDbContextFactory.SeedDbContext(context);
        var service = new SpecializationService(context, Mock.Of<ILogger<SpecializationService>>());

        var result = await service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Cardiologia");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("Spec_GetById_NotFound");
        TestDbContextFactory.SeedDbContext(context);
        var service = new SpecializationService(context, Mock.Of<ILogger<SpecializationService>>());

        var result = await service.GetByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldAdd()
    {
        var context = TestDbContextFactory.CreateDbContext("Spec_Add");
        var service = new SpecializationService(context, Mock.Of<ILogger<SpecializationService>>());

        var result = await service.AddAsync(new Specialization { Name = "Neurologia" });

        result.Should().NotBeNull();
        result!.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdate()
    {
        var context = TestDbContextFactory.CreateDbContext("Spec_Update");
        TestDbContextFactory.SeedDbContext(context);
        var service = new SpecializationService(context, Mock.Of<ILogger<SpecializationService>>());
        var spec = await service.GetByIdAsync(1);
        spec!.Name = "Cardiologia Avançada";

        await service.UpdateAsync(spec);

        var updated = await service.GetByIdAsync(1);
        updated!.Name.Should().Be("Cardiologia Avançada");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemove()
    {
        var context = TestDbContextFactory.CreateDbContext("Spec_Delete");
        TestDbContextFactory.SeedDbContext(context);
        var service = new SpecializationService(context, Mock.Of<ILogger<SpecializationService>>());

        await service.DeleteAsync((await service.GetByIdAsync(1))!);

        context.Specializations.Count().Should().Be(1);
    }
}
