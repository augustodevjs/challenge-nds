using Todo.Services.DTO;

namespace Todo.Services.Interfaces;

public interface IUserService
{
    Task Create(UserDTO userDto);
}