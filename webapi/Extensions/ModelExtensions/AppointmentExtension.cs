using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class AppointmentExtension
{
    public static AppointmentViewModel ToViewModel(this Appointment appointment)
    {
        return new AppointmentViewModel
        {
            Id = appointment.Id,
            Date = appointment.Date,
            Protocol = appointment.Protocol,
            Notes = appointment.Notes,
            Status = appointment.Status,
            User = appointment.User!.ToViewModel(),
            Doctor = appointment.Doctor!.ToViewModel(),
            MedicalCenter = appointment.MedicalCenter!.ToViewModel(),
            AppointmentRating = appointment.AppointmentRating?.ToViewModel(),
            CreatedAt = appointment.CreatedAt,
            UpdatedAt = appointment.UpdatedAt,
        };
    }
}