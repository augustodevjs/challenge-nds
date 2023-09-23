using FluentValidation;
using FluentValidation.Results;

namespace Todo.Application.DTO.V1.InputModel;

public class RegisterInputModel
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
    
    public bool Validar(out ValidationResult validationResult)
    {
        var validator = new InlineValidator<RegisterInputModel>();
        
        validator
            .RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("O campo Nome não pode ser deixado vazio.")
            .Length(3, 150)
            .WithMessage("O campo Nome precisa ter entre {MinLength} e {MaxLength} caracteres.");

        validator
            .RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("O campo Email não pode ser deixado vazio.")
            .EmailAddress()
            .WithMessage("O Campo Email fornecido não é válido.")
            .Length(3, 100)
            .WithMessage("O campo Email deve conter entre {MinLength} e {MaxLength} caracteres.");

        validator
            .RuleFor(c => c.Password)
            .NotEmpty()
            .WithMessage("O campo Senha não pode ser deixado vazio.")
            .Length(3, 250)
            .WithMessage("O campo Senha deve conter entre {MinLength} e {MaxLength} caracteres.");

        validator
            .RuleFor(c => c.ConfirmPassword)
            .NotEmpty()
            .WithMessage("O campo Confirmar Senha não pode ser deixado vazio.")
            .Equal(c => c.Password)
            .WithMessage("As senhas não coincidem.");

        validationResult = validator.Validate(this);
        return validationResult.IsValid;
    }
}