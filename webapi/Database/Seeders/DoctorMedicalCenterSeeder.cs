using WebApi.Models;
using WebApi.Database;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Database.Seeders;

public class DoctorMedicalCenterSeeder : ISeeder
{
    private readonly ApplicationDbContext _context;

    public DoctorMedicalCenterSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Seed()
    {
        if (!_context.DoctorMedicalCenters.Any())
        {
            var doctors = _context.Doctors.Take(200).ToList();
            var medicalCenters = _context.MedicalCenters.Take(10).ToList();

            if (doctors.Any() && medicalCenters.Any())
            {
                var relations = new List<DoctorMedicalCenter>();

                for (int i = 0; i < doctors.Count; i++)
                {
                    var centersToAssign = medicalCenters.Skip(i % medicalCenters.Count).Take((i % 2) + 1);

                    foreach (var center in centersToAssign)
                    {
                        relations.Add(new DoctorMedicalCenter
                        {
                            DoctorId = doctors[i].Id,
                            MedicalCenterId = center.Id,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                await _context.DoctorMedicalCenters.AddRangeAsync(relations);
                await _context.SaveChangesAsync();
            }
        }
    }
}