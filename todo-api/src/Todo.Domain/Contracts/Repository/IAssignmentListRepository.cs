using Todo.Domain.Models;

namespace Todo.Domain.Contracts.Repository;

public interface IAssignmentListRepository : IRepository<AssignmentList>
{
    Task<IPagedResult<AssignmentList>> Search(int? userId, string name, string description, int perPage = 10, int page = 1);
    Task<AssignmentList?> GetById(int? id, int? userId);
}