using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repositories;
using WebApi.Models.Dto;
using WebApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using WebApi.Helpers;
using WebApi.Extensions.ModelExtensions;
using WebApi.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Controllers;

[Authorize(Policy = "UserPolicy")]
[ApiController]
[Route("api/v1/public/exames")]
public class PublicMedicalExamController : ControllerBase
{
    private readonly IRepository<MedicalExam> _repository;
    private readonly ILogger<PublicMedicalExamController> _logger;
    private readonly ApplicationDbContext _context;

    public PublicMedicalExamController(IRepository<MedicalExam> repository, ILogger<PublicMedicalExamController> logger, ApplicationDbContext context)
    {
        _repository = repository;
        _logger = logger;
        _context = context;
    }

    protected object GetMedicalExamViewModel(MedicalExam model)
    {
        var viewmodel = new
        {
            model.Id,
            model.NomeExame,
            model.DescricaoExame,
            model.SetorResponsavel,
            model.TipoExame,
            model.PrazoEntrega,
            model.RequisitosPreparo,
            model.MaterialColeta,
        };

        return viewmodel;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicalExam>>> GetAllAsync(string? search = null)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            var all = await _repository.GetAllAsync();
            return StatusCode(200, ApiHelper.Ok(all.Select(GetMedicalExamViewModel)));
        }

        var list = await _repository.GetAllAsync();

        var viewModels = list
            .Where(m =>
                (!string.IsNullOrEmpty(m.NomeExame) && m.NomeExame.ToUpper().Contains(search.ToUpper())) ||
                (!string.IsNullOrEmpty(m.DescricaoExame) && m.DescricaoExame.ToUpper().Contains(search.ToUpper())) ||
                (!string.IsNullOrEmpty(m.SetorResponsavel) && m.SetorResponsavel.ToUpper().Contains(search.ToUpper())) ||
                (!string.IsNullOrEmpty(m.TipoExame) && m.TipoExame.ToUpper().Contains(search.ToUpper())) ||
                (!string.IsNullOrEmpty(m.PrazoEntrega) && m.PrazoEntrega.ToUpper().Contains(search.ToUpper())) ||
                (!string.IsNullOrEmpty(m.RequisitosPreparo) && m.RequisitosPreparo.ToUpper().Contains(search.ToUpper())) ||
                (!string.IsNullOrEmpty(m.MaterialColeta) && m.MaterialColeta.ToUpper().Contains(search.ToUpper()))

            )
            .Select(m => new MedicalExam
            {
                NomeExame = m.NomeExame,
                DescricaoExame = m.DescricaoExame,
                SetorResponsavel = m.SetorResponsavel,
                TipoExame = m.TipoExame,
                PrazoEntrega = m.PrazoEntrega,
                RequisitosPreparo = m.RequisitosPreparo,
                MaterialColeta = m.MaterialColeta
            })
            .ToList();

        return StatusCode(200, ApiHelper.Ok(viewModels));
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<MedicalExamViewModel>> GetByIdAsync(int id)
    {
        var model = await _repository.GetByIdAsync(id);

        if (model == null)
        {
            return StatusCode(404, ApiHelper.NotFound());
        }

        return StatusCode(200, ApiHelper.Ok(model.ToViewModel()));
    }

    [HttpPost]
    public async Task<ActionResult> AddAsync([FromBody] CreateMedicalExamDto dto)
    {
        try
        {
            ModelState.ClearValidationState(nameof(dto));

            if (!TryValidateModel(dto))
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));
            }

            var model = new MedicalExam
            {
                NomeExame = dto.NomeExame,
                DescricaoExame = dto.DescricaoExame,
                SetorResponsavel = dto.SetorResponsavel,
                TipoExame = dto.TipoExame,
                PrazoEntrega = dto.PrazoEntrega,
                RequisitosPreparo = dto.RequisitosPreparo,
                MaterialColeta = dto.MaterialColeta
            };

            await _repository.AddAsync(model);

            var result = await GetByIdAsync(model.Id);

            if (result.Result is ObjectResult objectResult)
            {
                objectResult.StatusCode = 201;
                return objectResult;
            }

            return StatusCode(201, result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateMedicalExamDto dto)
    {
        try
        {
            ModelState.ClearValidationState(nameof(dto));

            if (!TryValidateModel(dto))
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));
            }

            var model = await _repository.GetByIdAsync(id);

            if (model == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            model.NomeExame = dto.NomeExame ?? model.NomeExame;
            model.DescricaoExame = dto.DescricaoExame ?? model.DescricaoExame;
            model.SetorResponsavel = dto.SetorResponsavel ?? model.SetorResponsavel;
            model.TipoExame = dto.TipoExame ?? model.TipoExame;
            model.PrazoEntrega = dto.PrazoEntrega ?? model.PrazoEntrega;
            model.RequisitosPreparo = dto.RequisitosPreparo ?? model.RequisitosPreparo;
            model.MaterialColeta = dto.MaterialColeta ?? model.MaterialColeta;

            await _repository.UpdateAsync(model);

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            var model = await _repository.GetByIdAsync(id);

            if (model == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            await _repository.DeleteAsync(model);

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }
}
