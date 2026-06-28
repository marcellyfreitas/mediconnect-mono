using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class SpecializationExtension
{
    public static SpecializationViewModel ToViewModel(this Specialization Specialization)
    {
        return new SpecializationViewModel
        {
            Id = Specialization.Id,
            Name = Specialization.Name,
            Description = Specialization.Description,
            CreatedAt = Specialization.CreatedAt,
            UpdatedAt = Specialization.UpdatedAt,
        };
    }
}