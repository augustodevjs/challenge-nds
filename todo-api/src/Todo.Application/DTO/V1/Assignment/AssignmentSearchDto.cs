using Todo.Application.DTO.V1.Paged;

namespace Todo.Application.DTO.V1.Assignment;

public class AssignmentSearchDto : BaseSearchDto
{
    public string Description { get; set; }
    public DateTime? StartDeadline { get; set; }
    public DateTime? EndDeadline { get; set; }
    public bool? Concluded { get; set; }
}