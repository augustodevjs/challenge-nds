using System.Collections.ObjectModel;

namespace Todo.Domain.Models;

public class User : Entity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    // EF Relation
    public Collection<Assignment> Assignments { get; set; } = new();
    public Collection<AssignmentList> AssignmentLists { get; set; } = new();
}