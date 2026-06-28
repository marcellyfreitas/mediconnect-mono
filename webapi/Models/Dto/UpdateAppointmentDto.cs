using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class UpdateAppointmentDto
{
    public DateTime? Date { get; set; }

    public string? Protocol { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "O campo {0} deve ter no máximo {1} caracteres.")]
    public string? Notes { get; set; }

    public string? Status { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O campo {0} deve ser um valor positivo.")]
    public int? UserId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O campo {0} deve ser um valor positivo.")]
    public int? DoctorId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O campo {0} deve ser um valor positivo.")]
    public int? MedicalCenterId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O campo {0} deve ser um valor positivo.")]
    public int? AppointmentRatingId { get; set; }
}