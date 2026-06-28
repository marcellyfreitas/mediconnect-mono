using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class AddressExtension
{
    public static AddressViewModel ToViewModel(this Address address)
    {
        return new AddressViewModel
        {
            Id = address.Id,
            Logradouro = address.Logradouro,
            Cep = address.Cep,
            Bairro = address.Bairro,
            Cidade = address.Cidade,
            Estado = address.Estado,
            Pais = address.Pais,
            Numero = address.Numero,
            Complemento = address.Complemento,
            CreatedAt = address.CreatedAt,
            UpdatedAt = address.UpdatedAt,
        };
    }
}