using Todo.Domain.Models;
using Todo.Infra.Data.Context;
using Todo.Infra.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Contracts.Repository;

namespace Todo.Infra.Data.Repositories;

public class AssignmentRepository : Repository<Assignment>, IAssignmentRepository
{
    private readonly TodoDbContext _context;

    public AssignmentRepository(TodoDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Assignment?> GetById(Guid? id, Guid userId)
    {
        return await _context.Assignments.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
    }
}