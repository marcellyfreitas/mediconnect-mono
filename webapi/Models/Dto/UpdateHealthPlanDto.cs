using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class UpdateHealthPlanDto
{
    [StringLength(100, ErrorMessage = "O nome do plano deve ter no máximo 100 caracteres.")]
    public string? Name { get; set; } = string.Empty;

    public string? Coverage { get; set; } = string.Empty;
}
