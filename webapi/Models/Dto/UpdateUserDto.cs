using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class UpdateUserDto
{
    [Display(Name = "nome")]
    [StringLength(100, ErrorMessage = "O campo '{0}' deve ter no máximo {1} caracteres.")]
    public string? Name { get; set; } = null!;

    [Display(Name = "email")]
    [EmailAddress(ErrorMessage = "O campo '{0}' deve conter um e-mail válido.")]
    public string? Email { get; set; } = null!;

    [Display(Name = "senha")]
    [MinLength(6, ErrorMessage = "O campo '{0}' deve ter pelo menos {1} caracteres.")]
    [MaxLength(50, ErrorMessage = "O campo '{0}' deve ter no máximo {1} caracteres.")]
    public string? Password { get; set; } = null!;

    [Display(Name = "cpf")]
    [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O campo '{0}' deve estar no formato 000.000.000-00.")]
    public string? Cpf { get; set; } = null!;


    [Display(Name = "AddressId")]
    public int? AddressId { get; set; }
}
