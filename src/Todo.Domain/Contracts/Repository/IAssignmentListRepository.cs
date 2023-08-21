using Todo.Domain.Models;

namespace Todo.Domain.Contracts.Repository;

public interface IAssignmentListRepository : IRepository<AssignmentList>
{
    Task<IPagedResult<AssignmentList>> Search(Guid userId, string name, int perPage = 10, int page = 1);
}