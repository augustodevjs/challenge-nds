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
    private readonly TodoDbContext _context;

    public AssignmentRepository(TodoDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IPagedResult<Assignment>> Search(string userId, AssignmentFilter filter, int perPage = 10,
        int page = 1, string? listId = null)
    {
        var query = _context.Assignments
            .AsNoTracking()
            .Where(c => c.UserId == Guid.Parse(userId))
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

    public async Task<Assignment?> GetById(string id, string userId)
    {
        return await _context.Assignments.FirstOrDefaultAsync(c =>
            c.Id == Guid.Parse(id) && c.UserId == Guid.Parse(userId));
    }

    private static void ApplyFilter(string userId, AssignmentFilter filter, ref IQueryable<Assignment> query,
        string? listId = null)
    {
        if (!string.IsNullOrWhiteSpace(filter.Description))
            query = query.Where(c => c.Description.Contains(filter.Description));

        if (filter.Concluded.HasValue)
            query = query.Where(c => c.Concluded == filter.Concluded.Value);

        if (filter.StartDeadline.HasValue)
            query = query.Where(c => c.Deadline != null && c.Deadline.Value >= filter.StartDeadline.Value);

        if (filter.EndDeadline.HasValue)
            query = query.Where(c => c.Deadline != null && c.Deadline.Value <= filter.EndDeadline.Value);

        if (!string.IsNullOrEmpty(listId))
        {
            query = query
                .Where(c => c.AssignmentListId == Guid.Parse(listId))
                .Where(c => c.AssignmentList.UserId == Guid.Parse(userId));
        }
    }
}