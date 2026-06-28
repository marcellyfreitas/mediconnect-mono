using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models;

[Table("agendamentos")]
public class Appointment
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public string Protocol { get; set; } = string.Empty;

    public string? Notes { get; set; } = string.Empty;

    public string Status { get; set; } = "Agendado";

    [ForeignKey("User")]
    public int UserId { get; set; }

    public User? User { get; set; }

    [ForeignKey("Doctor")]
    public int DoctorId { get; set; }

    public Doctor? Doctor { get; set; }

    [ForeignKey("MedicalCenter")]
    public int MedicalCenterId { get; set; }

    public MedicalCenter? MedicalCenter { get; set; }

    public AppointmentRating? AppointmentRating { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}
