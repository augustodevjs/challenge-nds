using System.Collections.ObjectModel;

namespace Todo.Domain.Models;

public class AssignmentList : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid UserId { get; set; }

    // EF Relation
    public virtual User User { get; set; }
    public virtual Collection<Assignment> Assignments { get; set; } = new();
}