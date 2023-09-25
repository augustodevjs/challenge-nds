using FluentValidation.Results;
using Todo.Domain.Contracts;

namespace Todo.Domain.Models;

public abstract class Entity : IEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public virtual bool Validar(out ValidationResult validationResult)
    {
        validationResult = new ValidationResult();
        return validationResult.IsValid;
    }
}