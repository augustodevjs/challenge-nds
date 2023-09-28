using Todo.Domain.Validators;
using FluentValidation.Results;
using System.Collections.ObjectModel;

namespace Todo.Domain.Models;

public class AssignmentList : Entity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int UserId { get; set; }

    // EF Relation
    public virtual User User { get; set; }
    public virtual Collection<Assignment> Assignments { get; set; } = new();
    
    public override bool Validar(out ValidationResult validationResult)
    {
        validationResult = new AssignmentListValidator().Validate(this);
        return validationResult.IsValid;
    }
}