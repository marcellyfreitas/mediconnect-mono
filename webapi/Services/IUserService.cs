using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Services;

public interface IUserService<T> : IService<T>
{
    Task<T?> GetByEmailAsync(string email, int? id = null);
}