using Todo.Domain.Models;

namespace Todo.Infra.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmail(string email);
}