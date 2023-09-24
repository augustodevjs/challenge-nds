namespace Todo.Application.DTO.V1.ViewModel;

public class AssignmentViewModel : Base.Base
{
    public string Description { get; set; } = null!;
    public int AssignmentListId { get; set; }
    public DateTime Deadline { get; set; }
    public bool Concluded { get; set; }
    public DateTime? ConcludedAt { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}