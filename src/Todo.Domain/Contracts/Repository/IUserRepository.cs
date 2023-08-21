using Todo.Domain.Models;

namespace Todo.Domain.Contracts.Repository;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmail(string email);
}