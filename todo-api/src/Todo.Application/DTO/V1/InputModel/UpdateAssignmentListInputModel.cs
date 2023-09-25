namespace Todo.Application.DTO.V1.InputModel;

public class UpdateAssignmentListInputModel : Base.Base
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}