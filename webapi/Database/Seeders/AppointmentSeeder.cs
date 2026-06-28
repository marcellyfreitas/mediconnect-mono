using WebApi.Models;
using WebApi.Helpers;
using Bogus;
using System.Linq;

namespace WebApi.Database.Seeders;

public class AppointmentSeeder : ISeeder
{
    private readonly ApplicationDbContext _context;
    private readonly Faker _faker;

    public AppointmentSeeder(ApplicationDbContext context)
    {
        _context = context;
        _faker = new Faker("pt_BR");
    }

    public async Task Seed()
    {
        if (!_context.Appointments.Any())
        {
            if (!_context.Users.Any() || !_context.Doctors.Any() || !_context.MedicalCenters.Any())
            {
                Console.WriteLine("Não é possível criar consultas - necessários usuários, médicos e unidades médicas");
                return;
            }

            var userIds = _context.Users.Select(u => u.Id).ToList();
            var doctorIds = _context.Doctors.Select(d => d.Id).ToList();
            var medicalCenterIds = _context.MedicalCenters.Select(m => m.Id).ToList();

            var appointmentFaker = new Faker<Appointment>()
                .RuleFor(a => a.Date, f => f.Date.Between(DateTime.Now.AddDays(-30), DateTime.Now.AddMonths(3)))
                .RuleFor(a => a.Protocol, _ => ProtocolHelper.GenerateProtocol())
                .RuleFor(a => a.Notes, f => f.PickRandom(new[] {
                    "Consulta de rotina",
                    "Retorno médico",
                    "Exame periódico",
                    "Acompanhamento tratamento",
                    "Check-up anual",
                    null
                }))
                .RuleFor(a => a.UserId, f => f.PickRandom(userIds))
                .RuleFor(a => a.DoctorId, f => f.PickRandom(doctorIds))
                .RuleFor(a => a.MedicalCenterId, f => f.PickRandom(medicalCenterIds))
                .RuleFor(a => a.Status, f => f.PickRandom(new[] { "Agendado", "Concluído", "Cancelado" })); // Use strings ou enum se tiver

            var appointments = appointmentFaker.Generate(50);

            try
            {
                await _context.Appointments.AddRangeAsync(appointments);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao criar consultas: {e.Message}");
                throw;
            }
        }
    }
}