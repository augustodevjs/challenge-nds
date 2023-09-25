using FluentValidation;
using Todo.Domain.Models;

namespace Todo.Domain.Validators;

public class AssignmentValidator : AbstractValidator<Assignment>
{
    public AssignmentValidator()
    {
        RuleFor(u => u.Description)
            .NotEmpty().WithMessage("O campo de descrição não pode ser deixado vazio.")
            .Length(3, 255).WithMessage("O campo de descrição deve conter entre 3 e 255 caracteres.");

        RuleFor(u => u.Deadline).NotEmpty().WithMessage("O campo de prazo final não pode ser deixado vazio.");
        
        RuleFor(u => u.AssignmentListId)
            .NotEmpty().WithMessage("O campo AssignmentListId não pode ser deixado vazio.");
    }
}