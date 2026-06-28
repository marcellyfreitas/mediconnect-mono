using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class AdministratorExtension
{
    public static AdministratorViewModel ToViewModel(this Administrator administrator)
    {
        return new AdministratorViewModel
        {
            Id = administrator.Id,
            Name = administrator.Name,
            Email = administrator.Email,
            CreatedAt = administrator.CreatedAt,
            UpdatedAt = administrator.UpdatedAt,
        };
    }
}