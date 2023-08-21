using Microsoft.EntityFrameworkCore;
using Todo.Domain.Contracts.Repository;
using Todo.Domain.Models;
using Todo.Infra.Data.Abstractions;
using Todo.Infra.Data.Context;

namespace Todo.Infra.Data.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly TodoDbContext _context;

    public UserRepository(TodoDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmail(string email)
    {
        var user = await _context.Users
            .Where(x => x.Email.ToLower() == email.ToLower())
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return user;
    }
}