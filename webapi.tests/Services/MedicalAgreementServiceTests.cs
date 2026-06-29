using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Models;
using WebApi.Services;
using webapi.tests.Helpers;

namespace webapi.tests.Services;

public class MedicalAgreementServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnAll()
    {
        var context = TestDbContextFactory.CreateDbContext("Agreement_GetAll");
        TestDbContextFactory.SeedDbContext(context);
        var service = new MedicalAgreementService(context, Mock.Of<ILogger<MedicalAgreementService>>());

        var result = await service.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ShouldReturn()
    {
        var context = TestDbContextFactory.CreateDbContext("Agreement_GetById_Exists");
        TestDbContextFactory.SeedDbContext(context);
        var service = new MedicalAgreementService(context, Mock.Of<ILogger<MedicalAgreementService>>());

        var result = await service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Convênio A");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("Agreement_GetById_NotFound");
        TestDbContextFactory.SeedDbContext(context);
        var service = new MedicalAgreementService(context, Mock.Of<ILogger<MedicalAgreementService>>());

        var result = await service.GetByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldAdd()
    {
        var context = TestDbContextFactory.CreateDbContext("Agreement_Add");
        var service = new MedicalAgreementService(context, Mock.Of<ILogger<MedicalAgreementService>>());

        var result = await service.AddAsync(new MedicalAgreement { Name = "Convênio Novo" });

        result.Should().NotBeNull();
        result!.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdate()
    {
        var context = TestDbContextFactory.CreateDbContext("Agreement_Update");
        TestDbContextFactory.SeedDbContext(context);
        var service = new MedicalAgreementService(context, Mock.Of<ILogger<MedicalAgreementService>>());
        var agreement = await service.GetByIdAsync(1);
        agreement!.Name = "Convênio Alterado";

        await service.UpdateAsync(agreement);

        var updated = await service.GetByIdAsync(1);
        updated!.Name.Should().Be("Convênio Alterado");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemove()
    {
        var context = TestDbContextFactory.CreateDbContext("Agreement_Delete");
        TestDbContextFactory.SeedDbContext(context);
        var service = new MedicalAgreementService(context, Mock.Of<ILogger<MedicalAgreementService>>());

        await service.DeleteAsync((await service.GetByIdAsync(1))!);

        context.MedicalAgreements.Count().Should().Be(1);
    }
}
