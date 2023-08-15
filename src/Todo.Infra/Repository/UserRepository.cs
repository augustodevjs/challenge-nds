using Todo.Domain.Models;
using Todo.Infra.Context;
using Todo.Infra.Interfaces;

namespace Todo.Infra.Repository;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(TodoDbContext context) : base(context)
    {
    }
    
    
}