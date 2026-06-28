using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class AppointmentRatingExtension
{
    public static AppointmentRatingViewModel ToViewModel(this AppointmentRating appointmentRating)
    {
        return new AppointmentRatingViewModel
        {
            Id = appointmentRating.Id,
            Rating = appointmentRating.Rating,
            Comment = appointmentRating.Comment,
            User = appointmentRating.User?.ToViewModel(),
            CreatedAt = appointmentRating.CreatedAt ?? DateTime.Now,
            UpdatedAt = appointmentRating.UpdatedAt ?? DateTime.Now,
        };
    }
}