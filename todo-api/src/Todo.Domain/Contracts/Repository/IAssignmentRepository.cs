using Todo.Domain.Filter;
using Todo.Domain.Models;

namespace Todo.Domain.Contracts.Repository;

public interface IAssignmentRepository : IRepository<Assignment>
{
    Task<Assignment?> GetById(int id, int? userId);

    Task<IPagedResult<Assignment>> Search(int? userId, AssignmentFilter filter, int perPage = 10,
        int page = 1, int? listId = null);
}