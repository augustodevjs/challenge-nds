using Todo.Services.DTO;
using Todo.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Todo.Services.Interfaces;

namespace Todo.API.Controllers;

[Route("api/v1")]
public class AuthController : MainController
{
    private readonly IAuthService _authService;

    public AuthController(
        IAuthService authService,
        INotificator notificador
    ) : base(notificador)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDTO loginDto)
    {
        var userToken = await _authService.Login(loginDto);
        return CustomResponse(userToken);
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(UserDTO userDto)
    {
        await _authService.Create(userDto);

        return CustomResponse(new
        {
            userDto.Name,
            userDto.Email,
        });
    }
}