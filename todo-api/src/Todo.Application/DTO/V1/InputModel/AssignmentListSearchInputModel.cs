using Todo.Application.DTO.V1.Base;

namespace Todo.Application.DTO.V1.InputModel;

public class AssignmentListSearchInputModel : BaseSearch
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}