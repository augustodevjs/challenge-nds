using Todo.Services.DTO;
using Todo.Services.DTO.Auth;

namespace Todo.Services.Interfaces;

public interface IAuthService
{
    Task<RegisterDto?> Create(UserDto userDto);
    Task<TokenDto?> Login(LoginDto loginDto);
}