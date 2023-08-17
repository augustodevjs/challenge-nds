using Todo.Services.DTO;
using Todo.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Todo.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

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
    [ProducesResponseType(typeof(TokenDTO), StatusCodes.Status200OK)]
    [ProducesResponseType( typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Login(LoginDTO loginDto)
    {
        var userToken = await _authService.Login(loginDto);
        return CustomResponse(userToken);
    }

    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register Account")]
    [ProducesResponseType(typeof(RegisterDTO), StatusCodes.Status200OK)]
    [ProducesResponseType( typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register(UserDTO userDto)
    {
        return CustomResponse(await _authService.Create(userDto));
    }
}