using Todo.Services.DTO;

namespace Todo.Services.Interfaces;

public interface IAuthService
{
    Task<RegisterDTO?> Create(UserDTO userDto);
    Task<TokenDTO?> Login(LoginDTO loginDto);
}