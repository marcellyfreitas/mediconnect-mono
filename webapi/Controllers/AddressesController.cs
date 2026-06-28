using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repositories;
using WebApi.Models.Dto;
using WebApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using WebApi.Helpers;
using WebApi.Extensions.ModelExtensions;

namespace WebApi.Controllers;

[Authorize(Policy = "AdminPolicy")]
[ApiController]
[Route("api/v1/enderecos")]
public class AddressesController : ControllerBase
{
    private readonly IRepository<Address> _repository;
    private readonly ILogger<AddressesController> _logger;

    public AddressesController(IRepository<Address> addressRepository, ILogger<AddressesController> logger)
    {
        _repository = addressRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AddressViewModel>>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();

        var viewModels = list.Select(a => a.ToViewModel()).ToList();

        return StatusCode(200, ApiHelper.Ok(viewModels));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AddressViewModel>> GetByIdAsync(int id)
    {
        var model = await _repository.GetByIdAsync(id);

        if (model == null)
        {
            return StatusCode(404, ApiHelper.NotFound());
        }

        return StatusCode(200, ApiHelper.Ok(model.ToViewModel()));
    }

    [HttpPost]
    public async Task<ActionResult> AddAsync([FromBody] CreateAddressDto dto)
    {
        try
        {
            ModelState.ClearValidationState(nameof(dto));

            if (!TryValidateModel(dto))
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));
            }

            var model = new Address
            {
                Logradouro = dto.Logradouro,
                Cep = dto.Cep,
                Bairro = dto.Bairro,
                Cidade = dto.Cidade,
                Estado = dto.Estado,
                Pais = dto.Pais,
                Numero = dto.Numero,
                Complemento = dto.Complemento
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
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateAddressDto dto)
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

            model.Logradouro = dto.Logradouro ?? model.Logradouro;
            model.Cep = dto.Cep ?? model.Cep;
            model.Bairro = dto.Bairro ?? model.Bairro;
            model.Cidade = dto.Cidade ?? model.Cidade;
            model.Estado = dto.Estado ?? model.Estado;
            model.Pais = dto.Pais ?? model.Pais;
            model.Numero = dto.Numero ?? model.Numero;
            model.Complemento = dto.Complemento ?? model.Complemento;

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