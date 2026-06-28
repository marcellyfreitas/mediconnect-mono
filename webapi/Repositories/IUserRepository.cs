using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Repositories;

public interface IUserRepository<T> : IRepository<T>
{
    Task<T?> GetByEmailAsync(string email, int? id = null);
}