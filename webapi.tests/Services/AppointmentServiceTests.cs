using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Models;
using WebApi.Services;
using webapi.tests.Helpers;

namespace webapi.tests.Services;

public class AppointmentServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllWithIncludes()
    {
        var context = TestDbContextFactory.CreateDbContext("Appointment_GetAll");
        TestDbContextFactory.SeedDbContext(context);
        var service = new AppointmentService(context, Mock.Of<ILogger<AppointmentService>>());

        var result = await service.GetAllAsync();

        result.Should().HaveCount(2);
        result.First().Doctor.Should().NotBeNull();
        result.First().MedicalCenter.Should().NotBeNull();
        result.First().User.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ShouldReturnWithIncludes()
    {
        var context = TestDbContextFactory.CreateDbContext("Appointment_GetById_Exists");
        TestDbContextFactory.SeedDbContext(context);
        var service = new AppointmentService(context, Mock.Of<ILogger<AppointmentService>>());

        var result = await service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Protocol.Should().Be("PROT-001");
        result.Doctor.Should().NotBeNull();
        result.User.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("Appointment_GetById_NotFound");
        TestDbContextFactory.SeedDbContext(context);
        var service = new AppointmentService(context, Mock.Of<ILogger<AppointmentService>>());

        var result = await service.GetByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldGenerateProtocol()
    {
        var context = TestDbContextFactory.CreateDbContext("Appointment_Add");
        var service = new AppointmentService(context, Mock.Of<ILogger<AppointmentService>>());
        var appointment = new Appointment { Date = DateTime.UtcNow.AddDays(5), UserId = 1, DoctorId = 1, MedicalCenterId = 1 };

        var result = await service.AddAsync(appointment);

        result.Should().NotBeNull();
        result!.Protocol.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAndSetUpdatedAt()
    {
        var context = TestDbContextFactory.CreateDbContext("Appointment_Update");
        TestDbContextFactory.SeedDbContext(context);
        var service = new AppointmentService(context, Mock.Of<ILogger<AppointmentService>>());
        var appointment = await service.GetByIdAsync(1);
        appointment!.Status = "Concluído";

        await service.UpdateAsync(appointment);

        var updated = await service.GetByIdAsync(1);
        updated!.Status.Should().Be("Concluído");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveAppointment()
    {
        var context = TestDbContextFactory.CreateDbContext("Appointment_Delete");
        TestDbContextFactory.SeedDbContext(context);
        var service = new AppointmentService(context, Mock.Of<ILogger<AppointmentService>>());

        await service.DeleteAsync((await service.GetByIdAsync(1))!);

        context.Appointments.Count().Should().Be(1);
    }
}
