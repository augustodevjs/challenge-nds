using Todo.Domain.Contracts.Repository;
using Todo.Domain.Models;
using Todo.Infra.Data.Context;
using Todo.Infra.Data.Abstractions;

namespace Todo.Infra.Data.Repositories;

public class AssignmentRepository : Repository<Assignment>, IAssignmentRepository
{
    public AssignmentRepository(TodoDbContext context) : base(context)
    {
    }
}