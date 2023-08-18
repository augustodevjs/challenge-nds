namespace Todo.Services.DTO.Auth;

public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }

    public LoginDto()
    {
        
    }

    public LoginDto(string name, string email, string password)
    {
        Email = email;
        Password = password;
    }
}