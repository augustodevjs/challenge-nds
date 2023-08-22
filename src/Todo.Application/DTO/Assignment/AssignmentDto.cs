namespace Todo.Application.DTO.Assignment;

public class AssignmentDto : BaseDto
{
    public string Description { get; set; }
    public Guid AssignmentListId { get; set; }
    public DateTime Deadline { get; set; }
    public bool Concluded { get; set; }
    public DateTime? ConcludedAt { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}