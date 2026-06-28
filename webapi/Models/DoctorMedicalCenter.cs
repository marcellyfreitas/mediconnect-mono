using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models;

[Table("medico_centro_medico")]
public class DoctorMedicalCenter
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Doctor")]
    public int DoctorId { get; set; }

    public Doctor Doctor { get; set; } = null!;

    [ForeignKey("MedicalCenter")]
    public int MedicalCenterId { get; set; }

    public MedicalCenter MedicalCenter { get; set; } = null!;

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}