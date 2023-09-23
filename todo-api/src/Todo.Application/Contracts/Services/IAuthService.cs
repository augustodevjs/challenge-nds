using Todo.Application.DTO.V1.InputModel;
using Todo.Application.DTO.V1.ViewModel;

namespace Todo.Application.Contracts.Services;

public interface IAuthService
{
    Task<TokenViewModel?> Login(LoginInputModel inputModel);
    Task<UserViewModel?> Register(RegisterInputModel inputModel);
}