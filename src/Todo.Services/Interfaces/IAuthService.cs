using Todo.Services.DTO;

namespace Todo.Services.Interfaces;

public interface IAuthService
{
    Task Create(UserDTO userDto);
    Task<bool> Login(UserDTO userDto);
}