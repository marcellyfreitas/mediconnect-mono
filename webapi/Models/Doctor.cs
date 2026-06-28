using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models;

[Table("medicos")]
public class Doctor
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string CPF { get; set; } = "";

    public string Email { get; set; } = "";

    public string CRM { get; set; } = "";

    [ForeignKey("Specialization")]
    public int SpecializationId { get; set; }

    public Specialization? Specialization { get; set; }

    public List<DoctorMedicalCenter>? DoctorMedicalCenters { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}