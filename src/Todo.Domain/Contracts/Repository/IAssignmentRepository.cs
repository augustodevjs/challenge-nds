using Todo.Domain.Filter;
using Todo.Domain.Models;

namespace Todo.Domain.Contracts.Repository;

public interface IAssignmentRepository : IRepository<Assignment>
{
    Task<Assignment?> GetById(string id, string userId);

    Task<IPagedResult<Assignment>> Search(string userId, AssignmentFilter filter, int perPage = 10,
        int page = 1, string? listId = null);
}