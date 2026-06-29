using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using WebApi.Database;
using WebApi.Models;

namespace webapi.tests.Helpers;

public static class TestDbContextFactory
{
    public static ApplicationDbContext CreateDbContext(string databaseName = "TestDb")
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        var configurationMock = new Mock<IConfiguration>();
        return new ApplicationDbContext(options, configurationMock.Object);
    }

    public static void SeedDbContext(ApplicationDbContext context)
    {
        if (!context.Addresses.Any())
        {
            context.Addresses.AddRange(
                new Address { Id = 1, Logradouro = "Rua A", Cep = "12345-678", Bairro = "Centro", Cidade = "São Paulo", Estado = "SP", Pais = "Brasil", Numero = "100" },
                new Address { Id = 2, Logradouro = "Rua B", Cep = "87654-321", Bairro = "Jardim", Cidade = "Rio de Janeiro", Estado = "RJ", Pais = "Brasil", Numero = "200" }
            );
        }

        if (!context.Specializations.Any())
        {
            context.Specializations.AddRange(
                new Specialization { Id = 1, Name = "Cardiologia", Description = "Cardiologia" },
                new Specialization { Id = 2, Name = "Dermatologia", Description = "Dermatologia" }
            );
        }

        if (!context.Doctors.Any())
        {
            context.Doctors.AddRange(
                new Doctor { Id = 1, Name = "Dr. João", Email = "joao@medico.com", CPF = "12345678901", CRM = "SP123456", SpecializationId = 1 },
                new Doctor { Id = 2, Name = "Dra. Maria", Email = "maria@medico.com", CPF = "98765432101", CRM = "SP654321", SpecializationId = 2 }
            );
        }

        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User { Id = 1, Name = "Carlos", Email = "carlos@email.com", Password = "hash123", Cpf = "11122233344" },
                new User { Id = 2, Name = "Ana", Email = "ana@email.com", Password = "hash456", Cpf = "55566677788" }
            );
        }

        if (!context.Administrators.Any())
        {
            context.Administrators.AddRange(
                new Administrator { Id = 1, Name = "Admin Principal", Email = "admin@admin.com", Password = "hash_admin" },
                new Administrator { Id = 2, Name = "Admin Secundário", Email = "admin2@admin.com", Password = "hash_admin2" }
            );
        }

        if (!context.MedicalCenters.Any())
        {
            context.MedicalCenters.AddRange(
                new MedicalCenter { Id = 1, Name = "Hospital Central", Email = "hc@email.com", PhoneNumber = "11999999999", AddressId = 1 },
                new MedicalCenter { Id = 2, Name = "Clínica Regional", Email = "cr@email.com", PhoneNumber = "11888888888", AddressId = 2 }
            );
        }

        if (!context.Appointments.Any())
        {
            context.Appointments.AddRange(
                new Appointment { Id = 1, Date = DateTime.UtcNow.AddDays(1), Protocol = "PROT-001", Status = "Agendado", UserId = 1, DoctorId = 1, MedicalCenterId = 1 },
                new Appointment { Id = 2, Date = DateTime.UtcNow.AddDays(2), Protocol = "PROT-002", Status = "Confirmado", UserId = 2, DoctorId = 2, MedicalCenterId = 2 }
            );
        }

        if (!context.AppointmentRatings.Any())
        {
            context.AppointmentRatings.AddRange(
                new AppointmentRating { Id = 1, Rating = 5, Comment = "Ótimo", UserId = 1, AppointmentId = 1 },
                new AppointmentRating { Id = 2, Rating = 4, Comment = "Bom", UserId = 2, AppointmentId = 2 }
            );
        }

        if (!context.HealthPlans.Any())
        {
            context.HealthPlans.AddRange(
                new HealthPlan { Id = 1, Name = "Plano Básico", Coverage = "Básico" },
                new HealthPlan { Id = 2, Name = "Plano Premium", Coverage = "Completo" }
            );
        }

        if (!context.MedicalAgreements.Any())
        {
            context.MedicalAgreements.AddRange(
                new MedicalAgreement { Id = 1, Name = "Convênio A" },
                new MedicalAgreement { Id = 2, Name = "Convênio B" }
            );
        }

        if (!context.MedicalExams.Any())
        {
            context.MedicalExams.AddRange(
                new MedicalExam { Id = 1, NomeExame = "Hemograma", DescricaoExame = "Exame de sangue", TipoExame = "Sangue" },
                new MedicalExam { Id = 2, NomeExame = "Raio-X", DescricaoExame = "Exame de imagem", TipoExame = "Imagem" }
            );
        }

        context.SaveChanges();
    }
}
