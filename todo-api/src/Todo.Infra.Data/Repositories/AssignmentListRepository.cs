﻿using Todo.Domain.Models;
using Todo.Domain.Contracts;
using Todo.Infra.Data.Paged;
using Todo.Infra.Data.Context;
using Todo.Infra.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Contracts.Repository;

namespace Todo.Infra.Data.Repositories;

public class AssignmentListRepository : Repository<AssignmentList>, IAssignmentListRepository
{
    public AssignmentListRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IPagedResult<AssignmentList>> Search(int? userId, string name, string description,
        int perPage = 10, int page = 1)
    {
        var query = Context.AssignmentLists
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(c => c.Name.Contains(name));

        if (!string.IsNullOrWhiteSpace(description))
            query = query.Where(c => c.Description.Contains(description));

        var result = new PagedResult<AssignmentList>
        {
            Items = await query.OrderBy(c => c.Name).Include(c => c.Assignments).Skip((page - 1) * perPage)
                .Take(perPage).ToListAsync(),
            Total = await query.CountAsync(),
            Page = page,
            PerPage = perPage
        };

        var pageCount = (double)result.Total / perPage;
        result.PageCount = (int)Math.Ceiling(pageCount);

        return result;
    }

    public async Task<AssignmentList?> GetById(int? id, int? userId)
    {
        return await Context.AssignmentLists
            .Include(c => c.Assignments)
            .FirstOrDefaultAsync(c => id != null && c.Id == id && c.UserId == userId);
    }
}