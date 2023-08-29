using Todo.Application.DTO.V1.Assignment;

namespace Todo.Application.DTO.V1.AssignmentList;

public class AssignmentListDto : BaseDto
{
    public string Name { get; set; }
    public string Description { get; set; }

    public List<AssignmentDto> Assignments { get; set; } = new();
}