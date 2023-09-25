namespace Todo.Application.DTO.V1.InputModel;

public class UpdateAssignmentInputModel : Base.Base
{
    public string Description { get; set; } = null!;
    public DateTime Deadline { get; set; }
    public int AssignmentListId { get; set; }
}