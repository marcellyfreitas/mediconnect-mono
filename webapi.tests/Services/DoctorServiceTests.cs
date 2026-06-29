using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Models;
using WebApi.Services;
using webapi.tests.Helpers;

namespace webapi.tests.Services;

public class DoctorServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldIncludeSpecialization()
    {
        var context = TestDbContextFactory.CreateDbContext("Doctor_GetAll");
        TestDbContextFactory.SeedDbContext(context);
        var service = new DoctorService(context, Mock.Of<ILogger<DoctorService>>());

        var result = await service.GetAllAsync();

        result.Should().HaveCount(2);
        result.First().Specialization.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ShouldReturnWithSpecialization()
    {
        var context = TestDbContextFactory.CreateDbContext("Doctor_GetById_Exists");
        TestDbContextFactory.SeedDbContext(context);
        var service = new DoctorService(context, Mock.Of<ILogger<DoctorService>>());

        var result = await service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Dr. João");
        result.Specialization.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("Doctor_GetById_NotFound");
        TestDbContextFactory.SeedDbContext(context);
        var service = new DoctorService(context, Mock.Of<ILogger<DoctorService>>());

        var result = await service.GetByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldAddDoctor()
    {
        var context = TestDbContextFactory.CreateDbContext("Doctor_Add");
        var service = new DoctorService(context, Mock.Of<ILogger<DoctorService>>());
        var doctor = new Doctor { Name = "Dr. Novo", Email = "novo@medico.com", CPF = "11111111111", CRM = "SP999", SpecializationId = 1 };

        var result = await service.AddAsync(doctor);

        result.Should().NotBeNull();
        result!.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateDoctor()
    {
        var context = TestDbContextFactory.CreateDbContext("Doctor_Update");
        TestDbContextFactory.SeedDbContext(context);
        var service = new DoctorService(context, Mock.Of<ILogger<DoctorService>>());
        var doctor = await service.GetByIdAsync(1);
        doctor!.Name = "Dr. João Updated";

        await service.UpdateAsync(doctor);

        var updated = await service.GetByIdAsync(1);
        updated!.Name.Should().Be("Dr. João Updated");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveDoctor()
    {
        var context = TestDbContextFactory.CreateDbContext("Doctor_Delete");
        TestDbContextFactory.SeedDbContext(context);
        var service = new DoctorService(context, Mock.Of<ILogger<DoctorService>>());

        await service.DeleteAsync((await service.GetByIdAsync(1))!);

        context.Doctors.Count().Should().Be(1);
    }
}
