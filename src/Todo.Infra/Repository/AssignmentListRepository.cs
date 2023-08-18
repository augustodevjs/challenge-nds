using Todo.Domain.Models;
using Todo.Infra.Context;
using Todo.Infra.Interfaces;

namespace Todo.Infra.Repository;

public class AssignmentListRepository : Repository<AssignmentList>, IAssignmentListRepository
{
    public AssignmentListRepository(TodoDbContext context) : base(context)
    {
    }
}