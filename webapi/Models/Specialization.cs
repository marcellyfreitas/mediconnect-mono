using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models;

[Table("especializacoes")]
public class Specialization
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string? Description { get; set; } = "";

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}
