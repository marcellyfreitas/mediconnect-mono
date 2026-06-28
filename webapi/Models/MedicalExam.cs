using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models;

[Table("exames")]
public class MedicalExam
{
    [Key]
    public int Id { get; set; }

    public string NomeExame { get; set; } = string.Empty;

    public string DescricaoExame { get; set; } = string.Empty;

    public string SetorResponsavel { get; set; } = string.Empty;

    public string TipoExame { get; set; } = string.Empty;

    public string PrazoEntrega { get; set; } = string.Empty;

    public string RequisitosPreparo { get; set; } = string.Empty;

    public string MaterialColeta { get; set; } = string.Empty;

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}
