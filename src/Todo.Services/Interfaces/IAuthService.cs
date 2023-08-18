using Todo.Services.DTO.Auth;

namespace Todo.Services.Interfaces;

public interface IAuthService
{
    Task<UserDto?> Register(RegisterDto registerDto);
    Task<TokenDto?> Login(LoginDto loginDto);
}