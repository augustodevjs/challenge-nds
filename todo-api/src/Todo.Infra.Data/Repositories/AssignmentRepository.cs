using Todo.Domain.Filter;
using Todo.Domain.Models;
using Todo.Domain.Contracts;
using Todo.Infra.Data.Paged;
using Todo.Infra.Data.Context;
using Todo.Infra.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Contracts.Repository;

namespace Todo.Infra.Data.Repositories;

public class AssignmentRepository : Repository<Assignment>, IAssignmentRepository
{
    public AssignmentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IPagedResult<Assignment>> Search(int? userId, AssignmentFilter filter, int perPage = 10,
        int page = 1, int? listId = null)
    {
        var query = Context.Assignments
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .AsQueryable();

        ApplyFilter(userId, filter, ref query, listId);

        var result = new PagedResult<Assignment>
        {
            Items = await query.Skip((page - 1) * perPage).Take(perPage).ToListAsync(),
            Total = await query.CountAsync(),
            Page = page,
            PerPage = perPage
        };

        var pageCount = (double)result.Total / perPage;
        result.PageCount = (int)Math.Ceiling(pageCount);

        return result;
    }

    public async Task<Assignment?> GetById(int id, int? userId)
    {
        return await Context.Assignments.FirstOrDefaultAsync(c =>
            c.Id == id && c.UserId == userId);
    }

    private static void ApplyFilter(int? userId, AssignmentFilter filter, ref IQueryable<Assignment> query,
        int? listId = null)
    {
        if (!string.IsNullOrWhiteSpace(filter.Description))
            query = query.Where(c => c.Description.Contains(filter.Description));

        if (filter.Concluded.HasValue)
            query = query.Where(c => c.Concluded == filter.Concluded.Value);

        if (filter.StartDeadline.HasValue)
            query = query.Where(c => c.Deadline != null && c.Deadline.Value >= filter.StartDeadline.Value);

        if (filter.EndDeadline.HasValue)
            query = query.Where(c => c.Deadline != null && c.Deadline.Value <= filter.EndDeadline.Value);

        if (listId.HasValue)
        {
            query = query
                .Where(c => c.AssignmentListId == listId)
                .Where(c => c.AssignmentList.UserId == userId);
        }
    }
}