using FluentValidation;
using FluentValidation.Results;

namespace Todo.Application.DTO.V1.InputModel;

public class UpdateAssignmentListInputModel : Base.Base
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    
    public bool Validar(out ValidationResult validationResult)
    {
        var validator = new InlineValidator<UpdateAssignmentListInputModel>();
        
        validator
            .RuleFor(c => c.Name)
            .NotEmpty().WithMessage("O nome da lista de tarefas precisa ser fornecido.");
        
        validator
            .RuleFor(c => c.Description)
            .NotEmpty().WithMessage("A descrição da lista de tarefas precisa ser fornecida.");
        
        validationResult = validator.Validate(this);
        return validationResult.IsValid;
    }
}