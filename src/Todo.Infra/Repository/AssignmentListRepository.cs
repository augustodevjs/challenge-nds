using Todo.Domain.Models;
using Todo.Infra.Context;

namespace Todo.Infra.Repository;

public class AssignmentListRepository : Repository<AssignmentList>
{
    public AssignmentListRepository(TodoDbContext context) : base(context)
    {
    }
}