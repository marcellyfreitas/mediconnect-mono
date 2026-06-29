using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Models;
using WebApi.Services;
using webapi.tests.Helpers;

namespace webapi.tests.Services;

public class MedicalExamServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnAll()
    {
        var context = TestDbContextFactory.CreateDbContext("Exam_GetAll");
        TestDbContextFactory.SeedDbContext(context);
        var service = new MedicalExamService(context, Mock.Of<ILogger<MedicalExamService>>());

        var result = await service.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ShouldReturn()
    {
        var context = TestDbContextFactory.CreateDbContext("Exam_GetById_Exists");
        TestDbContextFactory.SeedDbContext(context);
        var service = new MedicalExamService(context, Mock.Of<ILogger<MedicalExamService>>());

        var result = await service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.NomeExame.Should().Be("Hemograma");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ShouldReturnNull()
    {
        var context = TestDbContextFactory.CreateDbContext("Exam_GetById_NotFound");
        TestDbContextFactory.SeedDbContext(context);
        var service = new MedicalExamService(context, Mock.Of<ILogger<MedicalExamService>>());

        var result = await service.GetByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldAdd()
    {
        var context = TestDbContextFactory.CreateDbContext("Exam_Add");
        var service = new MedicalExamService(context, Mock.Of<ILogger<MedicalExamService>>());

        var result = await service.AddAsync(new MedicalExam { NomeExame = "Ultrassom", TipoExame = "Imagem" });

        result.Should().NotBeNull();
        result!.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdate()
    {
        var context = TestDbContextFactory.CreateDbContext("Exam_Update");
        TestDbContextFactory.SeedDbContext(context);
        var service = new MedicalExamService(context, Mock.Of<ILogger<MedicalExamService>>());
        var exam = await service.GetByIdAsync(1);
        exam!.NomeExame = "Hemograma Completo";

        await service.UpdateAsync(exam);

        var updated = await service.GetByIdAsync(1);
        updated!.NomeExame.Should().Be("Hemograma Completo");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemove()
    {
        var context = TestDbContextFactory.CreateDbContext("Exam_Delete");
        TestDbContextFactory.SeedDbContext(context);
        var service = new MedicalExamService(context, Mock.Of<ILogger<MedicalExamService>>());

        await service.DeleteAsync((await service.GetByIdAsync(1))!);

        context.MedicalExams.Count().Should().Be(1);
    }
}
