namespace Todo.Services.DTO.Auth;

public class UserDto : BaseDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public UserDto()
    {
        
    }

    public UserDto(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }
}