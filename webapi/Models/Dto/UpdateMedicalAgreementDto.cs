using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class UpdateMedicalAgreementDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
