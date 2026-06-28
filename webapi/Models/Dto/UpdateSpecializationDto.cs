using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class UpdateSpecializationDto
{
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.")]
    public string? Name { get; set; }

    [StringLength(500, ErrorMessage = "O campo {0} deve ter no máximo {1} caracteres.")]
    public string? Description { get; set; }
}