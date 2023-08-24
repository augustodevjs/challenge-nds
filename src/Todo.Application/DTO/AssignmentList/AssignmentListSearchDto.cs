using Todo.Application.DTO.Paged;

namespace Todo.Application.DTO.AssignmentList;

public class AssignmentListSearchDto : BaseSearchDto
{
    public string Name { get; set; }
    public string Description { get; set; }
}