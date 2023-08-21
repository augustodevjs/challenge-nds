using Todo.Domain.Contracts;
using Todo.Domain.Models;

namespace Todo.Infra.Data.Paged;

public class PagedResult<T> : IPagedResult<T> where T : Entity, new()
{
    public List<T> Items { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int PerPage { get; set; }
    public int PageCount { get; set; }
}