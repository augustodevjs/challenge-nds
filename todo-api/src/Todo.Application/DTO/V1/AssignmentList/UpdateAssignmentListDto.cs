namespace Todo.Application.DTO.V1.AssignmentList;

public class UpdateAssignmentListDto : BaseDto
{
    public string Name { get; set; }
    public string Description { get; set; }
}