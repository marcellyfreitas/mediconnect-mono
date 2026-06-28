using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Models.Dto;
using WebApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using System.Security.Claims;
using WebApi.Helpers;
using WebApi.Extensions.ModelExtensions;

namespace WebApi.Controllers;

[Authorize(Policy = "UserPolicy")]
[ApiController]
[Route("api/v1/public/agendamentos")]
public class PublicAppointmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PublicAppointmentsController> _logger;

    public PublicAppointmentsController(ApplicationDbContext context, ILogger<PublicAppointmentsController> logger)
    {
        _context = context;
        _logger = logger;
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

            MedicalCenter = new
            {
                model.MedicalCenter!.Id,
                model.MedicalCenter.Name,
                Address = new
                {
                    model.MedicalCenter.Address!.Id,
                    model.MedicalCenter.Address!.Logradouro,
                    model.MedicalCenter.Address!.Bairro,
                    model.MedicalCenter.Address!.Numero,
                    model.MedicalCenter.Address!.Cep,
                },
            },

            Rating = model.AppointmentRating != null ? new
            {
                model.AppointmentRating.Id,
                model.AppointmentRating.Rating,
                model.AppointmentRating.Comment
            } : null
        };

        return viewmodel;
    }

    protected async Task<User?> GetAuthenticatedUserAsync()
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return null;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
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
    public async Task<IActionResult> GetAllAppointmentAsync()
    {
        try
        {
            var user = await GetAuthenticatedUserAsync();

            var query = await _context.Appointments
                .Where(r => r.UserId == user!.Id)
                .OrderByDescending(a => a.Id)
                .Include(r => r.Doctor).ThenInclude(d => d!.Specialization)
                .Include(r => r.MedicalCenter).ThenInclude(m => m!.Address)
                .Include(r => r.AppointmentRating)
                .ToListAsync();

            var viewmodels = query.Select(a => GetAppintmentViewModel(a)).ToList();

            return StatusCode(200, ApiHelper.Ok(viewmodels));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAppointmentByIdAsync(int id)
    {
        try
        {
            var user = await GetAuthenticatedUserAsync();

            var model = await _context.Appointments
                .Where(r => r.Id == id)
                .Include(r => r.Doctor).ThenInclude(d => d!.Specialization)
                .Include(r => r.MedicalCenter).ThenInclude(m => m!.Address)
                .Include(r => r.AppointmentRating)
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            var viewmodel = GetAppintmentViewModel(model);

            return StatusCode(200, ApiHelper.Ok(viewmodel));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddAppointmentAsync([FromBody] CreatePublicAppointmentDto dto)
    {
        try
        {
            ModelState.ClearValidationState(nameof(dto));

            if (!TryValidateModel(dto))
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));
            }

            var user = await GetAuthenticatedUserAsync();

            var model = new Appointment
            {
                Date = dto.Date,
                Notes = dto.Notes,
                Status = "Agendado",
                UserId = user!.Id,
                DoctorId = dto.DoctorId,
                MedicalCenterId = dto.MedicalCenterId,
                Protocol = ProtocolHelper.GenerateProtocol(),
            };

            _context.Appointments.Add(model);
            await _context.SaveChangesAsync();

            return StatusCode(200, ApiHelper.Created());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError("erro", dto));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAppointmentAsync(int id, [FromBody] UpdateAppointmentDto dto)
    {
        try
        {
            ModelState.ClearValidationState(nameof(dto));

            if (!TryValidateModel(dto))
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));
            }

            var user = await GetAuthenticatedUserAsync();

            var model = await _context.Appointments.FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            model.Date = dto.Date ?? model.Date;
            model.Notes = dto.Notes ?? model.Notes;
            model.Status = dto.Notes ?? model.Status;
            model.DoctorId = dto.DoctorId ?? model.DoctorId;
            model.MedicalCenterId = dto.MedicalCenterId ?? model.MedicalCenterId;

            model.UpdatedAt = DateTime.UtcNow;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError("erro", dto));
        }
    }

    [HttpPut("{id}/cancelar")]
    public async Task<IActionResult> CancelAppointmentAsync(int id)
    {
        try
        {
            var user = await GetAuthenticatedUserAsync();

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


    [HttpPost("avaliacao")]
    public async Task<IActionResult> AddAppointmentRatingAsync([FromBody] CreateAppointmentRatingDto dto)
    {
        try
        {
            ModelState.ClearValidationState(nameof(dto));

            if (!TryValidateModel(dto))
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));
            }

            var user = await GetAuthenticatedUserAsync();

            var model = new AppointmentRating
            {
                Rating = dto.Rating,
                Comment = dto.Comment,
                UserId = dto.UserId,
                AppointmentId = dto.AppointmentId,
            };

            await _context.AppointmentRatings.AddAsync(model);
            await _context.SaveChangesAsync();

            return StatusCode(200, ApiHelper.Created());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError("erro", dto));
        }
    }

    [HttpPut("avaliacao/{id}")]
    public async Task<IActionResult> UpdateAppointmentRatingAsync(int id, [FromBody] UpdateAppointmentRatingDto dto)
    {
        try
        {
            ModelState.ClearValidationState(nameof(dto));

            if (!TryValidateModel(dto))
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));
            }

            var user = await GetAuthenticatedUserAsync();

            var model = await _context.AppointmentRatings.FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            model.Rating = dto.Rating ?? model.Rating;
            model.Comment = dto.Comment ?? model.Comment;

            model.UpdatedAt = DateTime.UtcNow;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError("erro", dto));
        }
    }
}