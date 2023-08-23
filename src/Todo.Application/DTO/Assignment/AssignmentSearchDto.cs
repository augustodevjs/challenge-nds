using Todo.Application.DTO.Paged;

namespace Todo.Application.DTO.Assignment;

public class AssignmentSearchDto : BaseSearchDto
{
    public string Description { get; set; }
    public DateTime? StartDeadline { get; set; }
    public DateTime? EndDeadline { get; set; }
    public bool? Concluded { get; set; }
    public string OrderBy { get; set; } = "description";
    public string OrderDir { get; set; } = "asc";
}