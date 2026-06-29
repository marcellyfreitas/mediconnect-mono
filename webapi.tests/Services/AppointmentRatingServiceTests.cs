using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Models;
using WebApi.Services;
using webapi.tests.Helpers;

namespace webapi.tests.Services;

public class AppointmentRatingServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRatings()
    {
        var context = TestDbContextFactory.CreateDbContext("Rating_GetAll");
        TestDbContextFactory.SeedDbContext(context);
        var service = new AppointmentRatingService(context, Mock.Of<ILogger<AppointmentRatingService>>());

        var result = await service.GetAllAsync();

        result.Should().HaveCount(2);
        result.First().Appointment.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ShouldReturnRating()
    {
        var context = TestDbContextFactory.CreateDbContext("Rating_GetById_Exists");
        TestDbContextFactory.SeedDbContext(context);
        var service = new AppointmentRatingService(context, Mock.Of<ILogger<AppointmentRatingService>>());

        var result = await service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Rating.Should().Be(5);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("Rating_GetById_NotFound");
        TestDbContextFactory.SeedDbContext(context);
        var service = new AppointmentRatingService(context, Mock.Of<ILogger<AppointmentRatingService>>());

        var result = await service.GetByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldAddRating()
    {
        var context = TestDbContextFactory.CreateDbContext("Rating_Add");
        var service = new AppointmentRatingService(context, Mock.Of<ILogger<AppointmentRatingService>>());
        var rating = new AppointmentRating { Rating = 3, Comment = "Regular", UserId = 1, AppointmentId = 1 };

        var result = await service.AddAsync(rating);

        result.Should().NotBeNull();
        result!.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateRating()
    {
        var context = TestDbContextFactory.CreateDbContext("Rating_Update");
        TestDbContextFactory.SeedDbContext(context);
        var service = new AppointmentRatingService(context, Mock.Of<ILogger<AppointmentRatingService>>());
        var rating = await service.GetByIdAsync(1);
        rating!.Comment = "Excelente";

        await service.UpdateAsync(rating);

        var updated = await service.GetByIdAsync(1);
        updated!.Comment.Should().Be("Excelente");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRating()
    {
        var context = TestDbContextFactory.CreateDbContext("Rating_Delete");
        TestDbContextFactory.SeedDbContext(context);
        var service = new AppointmentRatingService(context, Mock.Of<ILogger<AppointmentRatingService>>());

        await service.DeleteAsync((await service.GetByIdAsync(1))!);

        context.AppointmentRatings.Count().Should().Be(1);
    }
}
