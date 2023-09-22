using FluentValidation;
using FluentValidation.Results;

namespace Todo.Application.DTO.V1.Auth;

public class LoginDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    public bool Validar(out ValidationResult validationResult)
    {
        var validator = new InlineValidator<LoginDto>();
        
        validator
            .RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("O campo de email não pode ser deixado vazio.")
            .EmailAddress()
            .WithMessage("O email fornecido não é válido.")
            .Length(3, 100)
            .WithMessage("O campo de email deve conter entre {MinLength} e {MaxLength} caracteres.");

        validator
            .RuleFor(c => c.Password)
            .NotEmpty()
            .WithMessage("A senha não pode ser deixada vazia.")
            .Length(3, 250)
            .WithMessage("A senha deve conter entre {MinLength} e {MaxLength} caracteres.");

        validationResult = validator.Validate(this);
        return validationResult.IsValid;
    }
}