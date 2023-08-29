namespace Todo.Application.DTO.V1.Auth;

public class TokenDto
{
    public string accessToken { get; set; } 
    public double expiresIn { get; set; } 
}