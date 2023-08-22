using Todo.Application.DTO.Auth;

namespace Todo.Application.Contracts.Services;

public interface IAuthService
{
    Task<TokenDto?> Login(LoginDto loginDto);
    Task<UserDto?> Register(RegisterDto registerDto);
}