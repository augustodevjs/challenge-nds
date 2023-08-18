using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Todo.Services.DTO.Auth;

public class LoginDto
{
    [EmailAddress(ErrorMessage = "O campo Email está inválido")]
    [Required(ErrorMessage = "O campo Email não pode ser vazio")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo Email precisa ter entre {2} e {1} caracteres")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "O campo Password não pode ser vazio")]
    [StringLength(250, MinimumLength = 3, ErrorMessage = "O campo Password precisa ter entre {2} e {1} caracteres")]
    [PasswordPropertyText]
    public string Password { get; set; }
}