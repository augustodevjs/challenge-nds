using FluentValidation;
using Todo.Domain.Models;

namespace Todo.Domain.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("O campo {PropertyName} não pode ser vazio.")
            .Length(3, 150)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("O campo {PropertyName} não pode ser vazio.")
            .Length(3, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres")
            .Matches(
                @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
            .WithMessage("O campo {PropertyName} está inválido");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("O campo {PropertyName} não pode ser vazio.")
            .Length(3, 250)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
    }
}