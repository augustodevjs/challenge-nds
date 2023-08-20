using FluentValidation;
using Todo.Domain.Models;

namespace Todo.Domain.Validators;

public class AssignmentListValidator : AbstractValidator<AssignmentList>
{
    public AssignmentListValidator()
    {
        RuleFor(f => f.Name)
            .NotEmpty().WithMessage("O nome da lista de tarefas precisa ser fornecido.");

        RuleFor(f => f.Description)
            .NotEmpty().WithMessage("A descrição da lista de tarefas precisa ser fornecida.");
        
        RuleFor(c => c.UserId)
            .NotNull().WithMessage("O ID do usuário não pode ser nulo.")
            .NotEqual(string.Empty).WithMessage("O ID do usuário não pode estar vazio.");
    }
}