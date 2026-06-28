using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class CreatePublicAppointmentDto
{
    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public DateTime Date { get; set; }

    [StringLength(500, ErrorMessage = "O campo {0} deve ter no máximo {1} caracteres.")]
    public string? Notes { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O campo {0} deve ser um valor positivo.")]
    public int DoctorId { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O campo {0} deve ser um valor positivo.")]
    public int MedicalCenterId { get; set; }
}