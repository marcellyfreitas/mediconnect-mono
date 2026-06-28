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
[Route("api/v1/usuarios")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository<User> _repository;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserRepository<User> repository, ILogger<UsersController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserViewModel>>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        var viewModels = list.Select(u => u.ToViewModel()).ToList();

        return StatusCode(200, ApiHelper.Ok(viewModels));
    }

    [HttpGet("pesquisar")]
    public async Task<ActionResult<IEnumerable<UserViewModel>>> GetAllFilteredAsync(string search)
    {
        if (string.IsNullOrEmpty(search))
        {
            return StatusCode(200, ApiHelper.Ok());
        }

        var list = await _repository.GetAllAsync();

        search = search.ToLower();
        list = list.Where(u =>
            (!string.IsNullOrEmpty(u.Name) && u.Name.ToLower().Contains(search)) ||
            (!string.IsNullOrEmpty(u.Email) && u.Email.ToLower().Contains(search))
        ).ToList();

        var viewModels = list.Select(u => u.ToViewModel()).ToList();

        return StatusCode(200, ApiHelper.Ok(viewModels));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserViewModel>> GetByIdAsync([FromRoute] int id)
    {
        var model = await _repository.GetByIdAsync(id);

        if (model == null)
        {
            return StatusCode(404, ApiHelper.NotFound());
        }

        return StatusCode(200, ApiHelper.Ok(model.ToViewModel()));
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CreateUserDto dto)
    {
        try
        {
            ModelState.ClearValidationState(nameof(dto));

            if (!TryValidateModel(dto))
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));
            }

            var existingEmail = await _repository.GetByEmailAsync(dto.Email);

            if (existingEmail != null)
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(Array.Empty<int>(), "Email está em uso"));
            }

            var model = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Cpf = dto.Cpf,
                Password = dto.Password,
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

    [HttpPut("{Id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int Id, [FromBody] UpdateUserDto dto)
    {
        try
        {
            ModelState.ClearValidationState(nameof(dto));

            if (!TryValidateModel(dto))
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));
            }



            var model = await _repository.GetByIdAsync(Id);

            if (model == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            var existingEmail = await _repository.GetByEmailAsync(dto.Email!, model.Id);

            if (existingEmail != null)
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(Array.Empty<int>(), "Email está em uso"));
            }

            model.Name = dto.Name ?? model.Name;
            model.Email = dto.Email ?? model.Email;
            model.Cpf = dto.Cpf ?? model.Cpf;


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