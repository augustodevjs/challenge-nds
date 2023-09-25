using FluentValidation;
using Todo.Domain.Models;

namespace Todo.Domain.Validators;

public class AssignmentListValidator : AbstractValidator<AssignmentList>
{
    public AssignmentListValidator()
    {
        RuleFor(u => u.Name).NotEmpty().WithMessage("O nome da lista de tarefas precisa ser fornecido.");

        RuleFor(u => u.Description).NotEmpty().WithMessage("A descrição da lista de tarefas precisa ser fornecida.");
    }
}