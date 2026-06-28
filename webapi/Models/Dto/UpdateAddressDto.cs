using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class UpdateAddressDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo 'Logradouro' é obrigatório!")]
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