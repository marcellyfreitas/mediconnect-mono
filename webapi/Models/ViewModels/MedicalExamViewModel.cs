using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.ViewModels;

public class MedicalExamViewModel
{
    public int Id { get; set; }

    [Required]
    public string NomeExame { get; set; } = "";

    [Required]
    public string DescricaoExame { get; set; } = "";

    [Required]
    public string SetorResponsavel { get; set; } = "";

    [Required]
    public string TipoExame { get; set; } = "";

    [Required]
    public string PrazoEntrega { get; set; } = "";

    [Required]
    public string RequisitosPreparo { get; set; } = "";

    [Required]
    public string MaterialColeta { get; set; } = "";

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }
}