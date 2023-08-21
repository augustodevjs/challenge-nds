namespace Todo.Application.DTO.Paged;

public abstract class BaseSearchDto
{
    public int Page { get; set; } = 1;
    public int PerPage { get; set; } = 10;
}