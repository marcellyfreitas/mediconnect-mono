using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models.Dto

{
    public class UpdateMedicalExamDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo 'Nome do Exame' é obrigatório!")]
        [StringLength(100, ErrorMessage = "O campo 'Nome do Exame' deve ter no máximo {1} caracteres.")]
        public string NomeExame { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Descrição do Exame' é obrigatório!")]
        [StringLength(255, ErrorMessage = "O campo 'Descrição do Exame' deve ter no máximo {1} caracteres.")]
        public string DescricaoExame { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Setor Responsável' é obrigatório!")]
        [StringLength(255, ErrorMessage = "O campo 'Setor Responsável' deve ter no máximo {1} caracteres.")]
        public string SetorResponsavel { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Tipo do Exame' é obrigatório!")]
        [StringLength(255, ErrorMessage = "O campo 'Tipo do Exame' deve ter no máximo {1} caracteres.")]
        public string TipoExame { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Prazo Estimado de Entrega' é obrigatório!")]
        [StringLength(255, ErrorMessage = "O campo 'Prazo Estimado de Entrega' deve ter no máximo {1} caracteres.")]
        public string PrazoEntrega { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Requisitos de Preparo' é obrigatório!")]
        [StringLength(255, ErrorMessage = "O campo 'Requisitos de Preparo' deve ter no máximo {1} caracteres.")]
        public string RequisitosPreparo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Material de Coleta' é obrigatório!")]
        [StringLength(255, ErrorMessage = "O campo 'Material de Coleta' deve ter no máximo {1} caracteres.")]
        public string MaterialColeta { get; set; } = string.Empty;

    }
}