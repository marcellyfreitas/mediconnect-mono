using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class UpdateMedicalCenterDto
{
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.")]
    public string? Name { get; set; }

    [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "O campo {0} deve ser um número de telefone válido.")]
    public string? PhoneNumber { get; set; }

    [EmailAddress(ErrorMessage = "O campo {0} deve ser um endereço de email válido.")]
    public string? Email { get; set; }

    [StringLength(255, ErrorMessage = "O campo 'Logradouro' deve ter no máximo 255 caracteres.")]
    public string Logradouro { get; set; } = "";

    [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "O campo 'Cep' deve estar no formato 00000-000.")]
    public string Cep { get; set; } = "";

    [StringLength(255, ErrorMessage = "O campo {0} deve ter no máximo {1} caracteres.")]
    public string? Bairro { get; set; } = "";

    [StringLength(255, ErrorMessage = "O campo {0} deve ter no máximo {1} caracteres.")]
    public string? Cidade { get; set; } = "";

    [StringLength(255, ErrorMessage = "O campo {0} deve ter no máximo {1} caracteres.")]
    public string? Estado { get; set; } = "";

    [StringLength(255, ErrorMessage = "O campo {0} deve ter no máximo {1} caracteres.")]
    public string? Pais { get; set; } = "Brasil";

    [StringLength(255, ErrorMessage = "O campo {0} deve ter no máximo {1} caracteres.")]
    public string? Numero { get; set; } = "SN";

    [StringLength(255, ErrorMessage = "O campo {0} deve ter no máximo {1} caracteres.")]
    public string? Complemento { get; set; } = "";
}