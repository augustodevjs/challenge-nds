
using Todo.Application.DTO.Assignment;

namespace Todo.Application.DTO.AssignmentList;

public class AssignmentListDto : BaseDto
{
    public string Name { get; set; }
    public string Description { get; set; }

    public List<AssignmentDto> Assignments { get; set; } = new();
}