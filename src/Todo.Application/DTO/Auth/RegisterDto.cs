using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Todo.Application.DTO.Auth;

public class RegisterDto
{
    [Required(ErrorMessage = "O campo Nome não pode ser deixado vazio.")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "O campo Nome precisa ter entre {2} e {1} caracteres.")]
    public string Name { get; set; }

    [EmailAddress(ErrorMessage = "O Campo Email fornecido não é válido.")]
    [Required(ErrorMessage = "O campo Email não pode ser deixado vazio.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo Email deve conter entre {2} e {1} caracteres.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "O campo Senha não pode ser deixado vazio.")]
    [StringLength(250, MinimumLength = 3, ErrorMessage = "O campo Senha deve conter entre {2} e {1} caracteres.")]
    [PasswordPropertyText]
    public string Password { get; set; }

    [Required(ErrorMessage = "O campo Confirmar Senha não pode ser deixado vazio.")]
    [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
    public string ConfirmPassword { get; set; }

}