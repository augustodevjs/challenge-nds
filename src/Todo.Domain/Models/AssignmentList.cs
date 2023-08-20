using System.Collections.ObjectModel;

namespace Todo.Domain.Models;

public class AssignmentList : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string UserId { get; set; }

    // EF Relation
    public User User { get; set; }
    public Collection<Assignment> Assignments { get; set; } = new();
}