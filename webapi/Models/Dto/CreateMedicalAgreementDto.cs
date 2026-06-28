using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class CreateMedicalAgreementDto
{
    public string Name { get; set; } = string.Empty;
}
