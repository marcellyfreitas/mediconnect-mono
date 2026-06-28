using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models.ViewModels;

public class HealthPlanViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Coverage { get; set; } = string.Empty;

    public List<MedicalAgreement> MedicalAgreements { get; set; } = new List<MedicalAgreement>();

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}