using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;
public class LoginDto
{
    [Required(ErrorMessage = "Campo obrigatório! {0}")]
    [StringLength(100, ErrorMessage = "O campo {0} não pode ter mais que 100 caracteres")]
    [EmailAddress(ErrorMessage = "O campo {0} possui um formato de email inválido")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Campo obrigatório! {0}")]
    [StringLength(100, ErrorMessage = "O campo {0} não pode ter mais que 100 caracteres")]
    public string Password { get; set; } = "";
}