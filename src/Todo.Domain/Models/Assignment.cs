namespace Todo.Domain.Models;

public class Assignment : Entity
{
    public Guid UserId { get; set; }
    public Guid? AssignmentListId { get; set; }
    
    public DateTime Deadline { get; set; }
    public string Description { get; set; }
    public bool Concluded { get; private set; }
    public DateTime? ConcludedAt { get; private set; }
    
    // EF Relation
    public User User { get; set; }
    public AssignmentList AssignmentList { get; set; }

    public void SetConcluded()
    {
        Concluded = true;
        ConcludedAt = DateTime.Now;
    }

    public void SetUnConcluded()
    {
        Concluded = false;
        ConcludedAt = null;
    }
}