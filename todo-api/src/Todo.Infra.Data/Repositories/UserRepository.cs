using Todo.Domain.Models;
using Todo.Infra.Data.Context;
using Todo.Infra.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Contracts.Repository;

namespace Todo.Infra.Data.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmail(string email)
    {
        var user = await Context.Users.Where(x => x.Email.ToLower() == email.ToLower()).AsNoTracking()
            .FirstOrDefaultAsync();

        return user;
    }
}