using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models.Dto;

public class CreateUserDto
{
    [Display(Name = "Nome")]
    [Required(ErrorMessage = "Preenchimento do Campo '{0}' Obrigatório!")]
    [StringLength(100, ErrorMessage = "O campo '{0}' deve ter no máximo {1} caracteres.")]
    public string Name { get; set; } = null!;

    [Display(Name = "Email")]
    [Required(ErrorMessage = "Preenchimento do Campo '{0}' Obrigatório!")]
    [EmailAddress(ErrorMessage = "O campo '{0}' deve conter um e-mail válido.")]
    public string Email { get; set; } = null!;

    [Display(Name = "Senha")]
    [Required(ErrorMessage = "Preenchimento do Campo '{0}' Obrigatório!")]
    [MinLength(6, ErrorMessage = "O campo '{0}' deve ter pelo menos {1} caracteres.")]
    [MaxLength(50, ErrorMessage = "O campo '{0}' deve ter no máximo {1} caracteres.")]
    public string Password { get; set; } = null!;

    [Display(Name = "CPF")]
    [Required(ErrorMessage = "Preenchimento do Campo '{0}' Obrigatório!")]
    [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O campo '{0}' deve estar no formato 000.000.000-00.")]
    public string Cpf { get; set; } = null!;


    [Display(Name = "AddressId")]
    public int? AddressId { get; set; }
}
