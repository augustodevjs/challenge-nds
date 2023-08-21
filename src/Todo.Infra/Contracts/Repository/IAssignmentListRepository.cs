using Todo.Domain.Models;

namespace Todo.Infra.Contracts.Repository;

public interface IAssignmentListRepository : IRepository<AssignmentList>
{
    Task<IPagedResult<AssignmentList>> Search(Guid userId, string name, string description, int perPage = 10, int page = 1);
}