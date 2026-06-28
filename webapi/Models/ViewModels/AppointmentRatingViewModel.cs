namespace WebApi.Models.ViewModels;

public class AppointmentRatingViewModel
{
    public int Id { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public int AppointmentId { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public AppointmentViewModel? Appointment { get; set; }
    public UserViewModel? User { get; set; }
}