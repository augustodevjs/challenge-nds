using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Application.Contracts;
using Todo.Application.DTO.Auth;

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
    [SwaggerOperation(Summary = "Login - Authentication")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);
        
        var userToken = await _authService.Login(loginDto);
        return CustomResponse(userToken);
    }

    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register Account")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var registerUser = await _authService.Register(registerDto);
        return CustomResponse(registerUser);
    }
}