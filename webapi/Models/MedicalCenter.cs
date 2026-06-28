using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models;

[Table("centros_medicos")]
public class MedicalCenter
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string PhoneNumber { get; set; } = "";

    public string? Email { get; set; } = "";

    [ForeignKey("Address")]
    public int AddressId { get; set; }

    public Address? Address { get; set; }

    public List<DoctorMedicalCenter>? DoctorMedicalCenters { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}