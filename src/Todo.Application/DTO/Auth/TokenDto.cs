namespace Todo.Application.DTO.Auth;

public class TokenDto
{
    public string accessToken { get; set; } 
    public double expiresIn { get; set; } 
}