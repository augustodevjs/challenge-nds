using FluentValidation;
using FluentValidation.Results;

namespace Todo.Application.DTO.V1.InputModel;

public class UpdateAssignmentInputModel : Base.Base
{
    public string Description { get; set; } = null!;
    public DateTime Deadline { get; set; }
    public int AssignmentListId { get; set; }
    
    public bool Validar(out ValidationResult validationResult)
    {
        var validator = new InlineValidator<UpdateAssignmentInputModel>();
        
        validator
            .RuleFor(c => c.Description)
            .NotEmpty().WithMessage("O campo de descrição não pode ser deixado vazio.")
            .Length(3, 255).WithMessage("O campo de descrição deve conter entre 3 e 255 caracteres.");
        
        validator
            .RuleFor(c => c.Deadline)
            .NotEmpty().WithMessage("O campo de prazo final não pode ser deixado vazio.");

        validator
            .RuleFor(c => c.AssignmentListId)
            .NotEmpty().WithMessage("O campo AssignmentListId não pode ser deixado vazio.");

        validationResult = validator.Validate(this);
        return validationResult.IsValid;
    }
    
}