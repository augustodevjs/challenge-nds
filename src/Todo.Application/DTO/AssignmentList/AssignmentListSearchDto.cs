using Todo.Application.DTO.Paged;

namespace Todo.Application.DTO.AssignmentList;

public class AssignmentListSearchDto : BaseSearchDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string OrderBy { get; set; } = "description";
    public string OrderDir { get; set; } = "asc";
}