using Todo.Domain.Models;
using Todo.Domain.Contracts;
using Todo.Infra.Data.Paged;
using Todo.Infra.Data.Context;
using Todo.Infra.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Contracts.Repository;

namespace Todo.Infra.Data.Repositories;

public class AssignmentListRepository : Repository<AssignmentList>, IAssignmentListRepository
{
    private readonly TodoDbContext _context;

    public AssignmentListRepository(TodoDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IPagedResult<AssignmentList>> Search(Guid userId, string name, string description, int perPage = 10, int page = 1)
    {
        var query = _context.AssignmentLists
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(c => c.Name.Contains(name));
        
        if (!string.IsNullOrWhiteSpace(description))
            query = query.Where(c => c.Description.Contains(description));

        var result = new PagedResult<AssignmentList>
        {
            Items = await query.OrderBy(c => c.Name).Skip((page - 1) * perPage).Take(perPage).ToListAsync(),
            Total = await query.CountAsync(),
            Page = page,
            PerPage = perPage
        };

        var pageCount = (double)result.Total / perPage;
        result.PageCount = (int)Math.Ceiling(pageCount);

        return result;
    }
}