namespace Todo.Infra.Filter;

public class AssignmentFilter
{
    public string Description { get; set; }
    public DateTime? StartDeadline { get; set; }
    public DateTime? EndDeadline { get; set; }
    public bool? Concluded { get; set; }
    public string OrderBy { get; set; } = "description";
    public string OrderDir { get; set; } = "asc";
}