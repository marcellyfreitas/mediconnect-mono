using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models;

[Table("planos_saude")]
public class HealthPlan
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Coverage { get; set; } = string.Empty;

    public List<MedicalAgreement> MedicalAgreements { get; set; } = new List<MedicalAgreement>();

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}