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

[Authorize(Policy = "AdminPolicy")]
[ApiController]
[Route("api/v1/agendamentos")]
public class AppointmentController : ControllerBase
{
    private readonly IRepository<Appointment> _repository;
    private readonly ILogger<AppointmentController> _logger;
    private readonly ApplicationDbContext _context;

    public AppointmentController(IRepository<Appointment> repository, ILogger<AppointmentController> logger, ApplicationDbContext context)
    {
        _repository = repository;
        _logger = logger;
        _context = context;
    }

    protected object GetAppintmentViewModel(Appointment model)
    {
        var viewmodel = new
        {
            model.Id,
            model.Date,
            model.Status,
            model.Protocol,
            model.Notes,
            model.CreatedAt,

            Doctor = model.Doctor != null ? new
            {
                model.Doctor.Id,
                model.Doctor.Name,
                model.Doctor.CRM,
                Specialization = model.Doctor.Specialization != null ? new
                {
                    model.Doctor.Specialization.Id,
                    model.Doctor.Specialization.Name
                } : null
            } : null,

            MedicalCenter = model.MedicalCenter != null ? new
            {
                model.MedicalCenter.Id,
                model.MedicalCenter.Name,
                Address = model.MedicalCenter.Address != null ? new
                {
                    model.MedicalCenter.Address.Id,
                    model.MedicalCenter.Address.Logradouro,
                    model.MedicalCenter.Address.Bairro,
                    model.MedicalCenter.Address.Numero,
                    model.MedicalCenter.Address.Cep,
                } : null
            } : null,

            User = model.User != null ? new
            {
                model.User.Name,
                model.User.Email,
            } : null,

            Rating = model.AppointmentRating != null ? new
            {
                model.AppointmentRating.Id,
                model.AppointmentRating.Rating,
                model.AppointmentRating.Comment
            } : null
        };

        return viewmodel;
    }

    [HttpGet("medicos")]
    public async Task<IActionResult> GetMedicalOrSpecialization([FromQuery] string? search, int? especialidade = null)
    {
        try
        {
            var query = _context.Doctors
                .Include(d => d.DoctorMedicalCenters!)
                    .ThenInclude(dmc => dmc.MedicalCenter)
                        .ThenInclude(mc => mc.Address)
                .Include(d => d.Specialization)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d => EF.Functions.Like(d.Name.ToLower(), $"%{search.ToLower()}%"));
            }

            if (especialidade != null)
            {
                query = query.Where(d => d.Specialization != null && d.SpecializationId == especialidade);
            }

            var result = await query
                .OrderByDescending(d => d.Id)
                .SelectMany(d => d.DoctorMedicalCenters!, (doctor, dmc) => new
                {
                    // Dados do médico
                    DoctorId = doctor.Id,
                    doctor.Name,
                    doctor.Email,
                    doctor.CPF,
                    doctor.CRM,
                    doctor.SpecializationId,
                    Specialization = doctor.Specialization != null ? new
                    {
                        doctor.Specialization.Id,
                        doctor.Specialization.Name,
                        doctor.Specialization.Description
                    } : null,

                    MedicalCenter = new
                    {
                        dmc.MedicalCenter.Id,
                        dmc.MedicalCenter.Name,
                        dmc.MedicalCenter.Email,
                        dmc.MedicalCenter.PhoneNumber,
                        Address = new
                        {
                            dmc.MedicalCenter.Address!.Id,
                            dmc.MedicalCenter.Address!.Logradouro,
                            dmc.MedicalCenter.Address.Cep,
                            dmc.MedicalCenter.Address.Bairro,
                            dmc.MedicalCenter.Address.Cidade,
                            dmc.MedicalCenter.Address.Estado,
                            dmc.MedicalCenter.Address.Pais,
                            dmc.MedicalCenter.Address.Numero,
                            dmc.MedicalCenter.Address.Complemento,
                        }
                    }
                })
                .ToListAsync();

            return StatusCode(200, ApiHelper.Ok(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpGet("horarios")]
    public async Task<IActionResult> GetAllAppointmentHoursAsync([FromQuery] DateTime date, int doctorId, int medicalCenterId)
    {
        try
        {
            var hours = await _context.Appointments
                .OrderByDescending(a => a.Id)
                .Where(a => a.Date.Date == date.Date)
                .Where(a => a.DoctorId == doctorId)
                .Where(a => a.MedicalCenterId == medicalCenterId)
                .Select(a => a.Date)
                .ToListAsync();

            return StatusCode(200, ApiHelper.Ok(hours));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentViewModel>>> GetAllAsync(string search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return StatusCode(200, ApiHelper.Ok());
        }

        var list = await _repository.GetAllAsync();

        var viewModels = list
            .Where(m =>
                (m.Protocol != null && m.Protocol.ToUpper().Contains(search.ToUpper())) ||
                (m.User != null && m.User.Name != null && m.User.Name.ToUpper().Contains(search.ToUpper())) ||
                (m.User != null && m.User.Cpf != null &&
                m.User.Cpf.Replace(".", "").Replace("-", "").ToUpper().Contains(search.ToUpper()))
            )
            .Select(u => GetAppintmentViewModel(u))
            .ToList();

        return StatusCode(200, ApiHelper.Ok(viewModels));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentViewModel>> GetByIdAsync([FromRoute] int id)
    {
        var model = await _repository.GetByIdAsync(id);

        if (model == null)
        {
            return StatusCode(404, ApiHelper.NotFound());
        }

        return StatusCode(200, ApiHelper.Ok(model));
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CreateAppointmentDto dto)
    {
        try
        {
            ModelState.ClearValidationState(nameof(dto));

            if (!TryValidateModel(dto))
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));
            }

            var model = new Appointment
            {
                Date = dto.Date,
                Notes = dto.Notes,
                Status = dto.Status,
                UserId = dto.UserId,
                DoctorId = dto.DoctorId,
                MedicalCenterId = dto.MedicalCenterId,
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
    public async Task<IActionResult> UpdateAsync([FromRoute] int Id, [FromBody] UpdateAppointmentDto dto)
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

            model.Date = dto.Date ?? model.Date;
            model.Notes = dto.Notes ?? model.Notes;
            model.Status = dto.Status ?? model.Status;

            await _repository.UpdateAsync(model);

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }

    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
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

    [HttpPut("{id}/cancelar")]
    public async Task<IActionResult> CancelAppointmentAsync(int id)
    {
        try
        {
            var model = await _context.Appointments.FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            model.Notes = "Agendamento cancelado pelo usuário";
            model.Status = "Cancelada";

            model.UpdatedAt = DateTime.UtcNow;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError("erro"));
        }
    }

}