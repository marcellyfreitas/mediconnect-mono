using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class DoctorExtension
{    public static DoctorViewModel ToViewModel(this Doctor doctor)
    {
        return new DoctorViewModel
        {
            Id = doctor.Id,
            Name = doctor.Name,
            Email = doctor.Email,
            CPF = doctor.CPF,
            CRM = doctor.CRM,
            SpecializationId = doctor.SpecializationId,
            Specialization = doctor.Specialization?.ToViewModel(),
            MedicalCenters = (doctor.DoctorMedicalCenters?.Any() ?? false)
                ? doctor.DoctorMedicalCenters
                    .Select(dmc => new MedicalCenterViewModel
                    {
                        Id = dmc.MedicalCenter.Id,
                        Name = dmc.MedicalCenter.Name,
                        PhoneNumber = dmc.MedicalCenter.PhoneNumber,
                        Email = dmc.MedicalCenter.Email,
                        Address = dmc.MedicalCenter.Address?.ToViewModel(),
                    })
                    .ToList()
                : new List<MedicalCenterViewModel>(),
            CreatedAt = doctor.CreatedAt,
            UpdatedAt = doctor.UpdatedAt,
        };
    }
}
