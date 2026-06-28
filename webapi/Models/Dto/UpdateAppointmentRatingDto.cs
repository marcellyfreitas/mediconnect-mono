using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class UpdateAppointmentRatingDto
{
    [Range(1, 5, ErrorMessage = "O campo {0} deve ser um valor entre {1} e {2}.")]
    public int? Rating { get; set; }

    [StringLength(500, ErrorMessage = "O campo {0} deve ter no máximo {1} caracteres.")]
    public string? Comment { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O campo {0} deve ser um valor positivo.")]
    public int? AppointmentId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O campo {0} deve ser um valor positivo.")]
    public int? UserId { get; set; }
}