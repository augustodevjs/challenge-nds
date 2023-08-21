using Todo.Application.DTO.Auth;

namespace Todo.Application.Contracts;

public interface IAuthService
{
    Task<UserDto?> Register(RegisterDto registerDto);
    Task<TokenDto?> Login(LoginDto loginDto);
}