using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Models;
using WebApi.Services;
using webapi.tests.Helpers;

namespace webapi.tests.Services;

public class HealthPlanServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllWithMedicalAgreements()
    {
        var context = TestDbContextFactory.CreateDbContext("HealthPlan_GetAll");
        TestDbContextFactory.SeedDbContext(context);
        var service = new HealthPlanService(context, Mock.Of<ILogger<HealthPlanService>>());

        var result = await service.GetAllAsync();

        result.Should().HaveCount(2);
        result.First().MedicalAgreements.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ShouldReturnPlan()
    {
        var context = TestDbContextFactory.CreateDbContext("HealthPlan_GetById_Exists");
        TestDbContextFactory.SeedDbContext(context);
        var service = new HealthPlanService(context, Mock.Of<ILogger<HealthPlanService>>());

        var result = await service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Plano Básico");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("HealthPlan_GetById_NotFound");
        TestDbContextFactory.SeedDbContext(context);
        var service = new HealthPlanService(context, Mock.Of<ILogger<HealthPlanService>>());

        var result = await service.GetByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldAddPlan()
    {
        var context = TestDbContextFactory.CreateDbContext("HealthPlan_Add");
        var service = new HealthPlanService(context, Mock.Of<ILogger<HealthPlanService>>());

        var result = await service.AddAsync(new HealthPlan { Name = "Plano Novo", Coverage = "Total" });

        result.Should().NotBeNull();
        result!.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdatePlan()
    {
        var context = TestDbContextFactory.CreateDbContext("HealthPlan_Update");
        TestDbContextFactory.SeedDbContext(context);
        var service = new HealthPlanService(context, Mock.Of<ILogger<HealthPlanService>>());
        var plan = await service.GetByIdAsync(1);
        plan!.Coverage = "Completo";

        await service.UpdateAsync(plan);

        var updated = await service.GetByIdAsync(1);
        updated!.Coverage.Should().Be("Completo");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemovePlan()
    {
        var context = TestDbContextFactory.CreateDbContext("HealthPlan_Delete");
        TestDbContextFactory.SeedDbContext(context);
        var service = new HealthPlanService(context, Mock.Of<ILogger<HealthPlanService>>());

        await service.DeleteAsync((await service.GetByIdAsync(1))!);

        context.HealthPlans.Count().Should().Be(1);
    }
}
