using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class MedicalExamExtension
{
    public static MedicalExamViewModel ToViewModel(this MedicalExam medicalExam)
    {
        return new MedicalExamViewModel
        {
            Id = medicalExam.Id,
            NomeExame = medicalExam.NomeExame ?? "",
            DescricaoExame = medicalExam.DescricaoExame ?? "",
            SetorResponsavel = medicalExam.SetorResponsavel ?? "",
            TipoExame = medicalExam.TipoExame ?? "",
            PrazoEntrega = medicalExam.PrazoEntrega ?? "",
            RequisitosPreparo = medicalExam.RequisitosPreparo ?? "",
            MaterialColeta = medicalExam.MaterialColeta ?? "",
            CreatedAt = medicalExam.CreatedAt ?? DateTime.MinValue,
            UpdatedAt = medicalExam.UpdatedAt ?? DateTime.MinValue,
        };
    }
}