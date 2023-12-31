﻿using Todo.Domain.Validators;
using FluentValidation.Results;

namespace Todo.Domain.Models;

public class Assignment : Entity
{
    public string Description { get; set; } = null!;
    public int UserId { get; set; }
    public int? AssignmentListId { get; set; }
    public DateTime? Deadline { get; set; }
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

    public void SetUnconcluded()
    {
        Concluded = false;
        ConcludedAt = null;
    }

    public override bool Validar(out ValidationResult validationResult)
    {
        validationResult = new AssignmentValidator().Validate(this);
        return validationResult.IsValid;
    }
}