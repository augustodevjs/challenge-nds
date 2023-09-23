namespace Todo.Application.DTO.V1.Base;

public class BaseSearch
{
    public int Page { get; set; } = 1;
    public int PerPage { get; set; } = 10;
}