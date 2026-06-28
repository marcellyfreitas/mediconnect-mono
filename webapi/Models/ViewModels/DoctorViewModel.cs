namespace WebApi.Models.ViewModels;

public class DoctorViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string CPF { get; set; } = "";

    public string Email { get; set; } = "";    public string CRM { get; set; } = "";

    public int SpecializationId { get; set; }

    public SpecializationViewModel? Specialization { get; set; } = null!;

    public List<MedicalCenterViewModel>? MedicalCenters { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}