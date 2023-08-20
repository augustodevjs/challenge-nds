using Todo.Services.DTO.AssignmentList;

namespace Todo.Services.DTO.Assignment;

public class AssignmentDto : BaseDto
{
    public string Description { get; set; }
    public string? AssignmentListId { get; set; }
    public DateTime? Deadline { get; set; }
    public bool Concluded { get; set; }
    public DateTime? ConcludedAt { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public AssignmentListDto AssignmentList { get; set; }
}