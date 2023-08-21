using Todo.Domain.Models;
using Todo.Infra.Data.Abstractions;
using Todo.Infra.Data.Context;

namespace Todo.Infra.Data.Repositories;

public class AssignmentRepository : Repository<Assignment>
{
    public AssignmentRepository(TodoDbContext context) : base(context)
    {
    }
}