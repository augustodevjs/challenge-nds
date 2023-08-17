using Todo.Domain.Models;
using Todo.Services.DTO;

namespace Todo.Services.Interfaces;

public interface IAuthService
{
    Task Create(UserDTO userDto);
    Task<string?> Login(LoginDTO loginDto);
}