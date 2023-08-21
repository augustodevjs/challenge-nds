using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Todo.Application.DTO.Auth;

public class LoginDto
{
    [EmailAddress(ErrorMessage = "O email fornecido não é válido.")]
    [Required(ErrorMessage = "O campo de email não pode ser deixado vazio.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo de email deve conter entre {2} e {1} caracteres.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha não pode ser deixada vazia.")]
    [StringLength(250, MinimumLength = 3, ErrorMessage = "A senha deve conter entre {2} e {1} caracteres.")]
    [PasswordPropertyText]
    public string Password { get; set; }
}