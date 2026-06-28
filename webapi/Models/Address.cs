using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models;

[Table("enderecos")]
public class Address
{
    [Key]
    public int Id { get; set; }

    public string Logradouro { get; set; } = "";

    public string? Cep { get; set; } = "";

    public string? Bairro { get; set; } = "";

    public string? Cidade { get; set; } = "";

    public string? Estado { get; set; } = "";

    public string? Pais { get; set; } = "Brasil";

    public string? Numero { get; set; } = "";

    public string? Complemento { get; set; } = "";

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}
