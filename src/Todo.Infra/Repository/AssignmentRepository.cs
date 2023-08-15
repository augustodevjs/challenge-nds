using Todo.Domain.Models;
using Todo.Infra.Context;

namespace Todo.Infra.Repository;

public class AssignmentRepository : Repository<Assignment>
{
    public AssignmentRepository(TodoDbContext context) : base(context)
    {
    }
}