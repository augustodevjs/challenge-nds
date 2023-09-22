using Todo.API.Responses;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.DTO.V1.Auth;
using Todo.Application.Notifications;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Application.Contracts.Services;

namespace Todo.API.Controllers;

[Route("auth")]
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
    [ProducesResponseType(typeof(UnauthorizedObjectResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await _authService.Login(loginDto);
        return token != null ? OkResponse(token) : Unauthorized(new[] { "Usuário e/ou senha incorretos" });
    }

    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register Account")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var registerUser = await _authService.Register(registerDto);
        return OkResponse(registerUser);
    }
}