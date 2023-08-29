using Todo.Application.DTO.V1.Paged;

namespace Todo.Application.DTO.V1.AssignmentList;

public class AssignmentListSearchDto : BaseSearchDto
{
    public string Name { get; set; }
    public string Description { get; set; }
}