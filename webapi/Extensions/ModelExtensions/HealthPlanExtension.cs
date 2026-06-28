using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class HealthPlanExtension
{
    public static HealthPlanViewModel ToViewModel(this HealthPlan healthPlan)
    {
        return new HealthPlanViewModel
        {
            Id = healthPlan.Id,
            Name = healthPlan.Name,
            Coverage = healthPlan.Coverage,
            MedicalAgreements = healthPlan.MedicalAgreements,
            CreatedAt = healthPlan.CreatedAt,
            UpdatedAt = healthPlan.UpdatedAt,
        };
    }
}