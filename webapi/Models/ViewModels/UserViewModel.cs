namespace WebApi.Models.ViewModels;

public class UserViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Email { get; set; } = "";

    public string Cpf { get; set; } = "";

    public AddressViewModel? address { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
