using Todo.Domain.Models;

namespace Todo.Infra.Contracts.Repository;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmail(string email);
}