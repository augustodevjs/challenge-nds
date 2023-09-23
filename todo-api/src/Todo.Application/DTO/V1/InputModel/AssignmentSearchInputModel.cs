using Todo.Application.DTO.V1.Base;

namespace Todo.Application.DTO.V1.InputModel;

public class AssignmentSearchInputModel : BaseSearch
{
    public string Description { get; set; } = null!;
    public DateTime? StartDeadline { get; set; }
    public DateTime? EndDeadline { get; set; }
    public bool? Concluded { get; set; }
}