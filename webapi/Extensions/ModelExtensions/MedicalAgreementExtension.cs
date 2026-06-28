using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class MedicalAgreementExtension
{
    public static MedicalAgreementViewModel ToViewModel(this MedicalAgreement medicalAgreement)
    {
        return new MedicalAgreementViewModel
        {
            Id = medicalAgreement.Id,
            Name = medicalAgreement.Name,
            CreatedAt = medicalAgreement.CreatedAt,
            UpdatedAt = medicalAgreement.UpdatedAt,
        };
    }
}