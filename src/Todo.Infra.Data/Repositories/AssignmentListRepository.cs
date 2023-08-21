using Todo.Domain.Contracts.Repository;
using Todo.Domain.Models;
using Todo.Infra.Data.Abstractions;
using Todo.Infra.Data.Context;

namespace Todo.Infra.Data.Repositories;

public class AssignmentListRepository : Repository<AssignmentList>, IAssignmentListRepository
{
    private readonly TodoDbContext _context;
    
    public AssignmentListRepository(TodoDbContext context) : base(context)
    {
        _context = context;
    }
}