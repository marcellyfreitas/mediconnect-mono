using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class MedicalCenterExtension
{
    public static MedicalCenterViewModel ToViewModel(this MedicalCenter medicalCenter)
    {
        return new MedicalCenterViewModel
        {
            Id = medicalCenter.Id,
            Name = medicalCenter.Name,
            PhoneNumber = medicalCenter.PhoneNumber,
            Email = medicalCenter.Email,
            Doctors = (medicalCenter.DoctorMedicalCenters?.Any() ?? false)
                ? medicalCenter.DoctorMedicalCenters
                    .Select(dmc => new DoctorViewModel
                    {
                        Id = dmc.Doctor.Id,
                        Name = dmc.Doctor.Name,
                        Email = dmc.Doctor.Email,
                        CRM = dmc.Doctor.CRM,
                    })
                    .ToList()
                : new List<DoctorViewModel>(),
            Address = medicalCenter.Address != null ? new AddressViewModel
            {
                Id = medicalCenter.Address.Id,
                Logradouro = medicalCenter.Address.Logradouro,
                Bairro = medicalCenter.Address.Bairro,
                Numero = medicalCenter.Address.Numero,
                Cidade = medicalCenter.Address.Cidade,
                Estado = medicalCenter.Address.Estado,
                Pais = medicalCenter.Address.Pais,
                Cep = medicalCenter.Address.Cep,
                Complemento = medicalCenter.Address.Complemento,
            } : null,
            CreatedAt = medicalCenter.CreatedAt,
            UpdatedAt = medicalCenter.UpdatedAt,
        };
    }
}
