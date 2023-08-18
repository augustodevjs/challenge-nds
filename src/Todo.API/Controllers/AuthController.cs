using Todo.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Todo.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Services.DTO.Auth;

namespace Todo.API.Controllers;

[Route("Auth")]
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
    [SwaggerOperation(Summary = "Login")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Login(LoginDto loginDto)
    {
        var userToken = await _authService.Login(loginDto);
        return CustomResponse(userToken);
    }

    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register Account")]
    [ProducesResponseType(typeof(RegisterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register(UserDto userDto)
    {
        var registerUser = await _authService.Create(userDto);
        return CustomResponse(registerUser);
    }
}