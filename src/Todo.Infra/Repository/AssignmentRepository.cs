using Todo.Domain.Models;
using Todo.Infra.Context;
using Todo.Infra.Contracts;

namespace Todo.Infra.Repository;

public class AssignmentRepository : Repository<Assignment>
{
    public AssignmentRepository(TodoDbContext context) : base(context)
    {
    }
}