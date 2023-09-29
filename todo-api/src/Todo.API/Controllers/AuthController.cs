using Todo.API.Responses;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.Notifications;
using Todo.Application.DTO.V1.ViewModel;
using Todo.Application.DTO.V1.InputModel;
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
    [ProducesResponseType(typeof(TokenViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login([FromBody] LoginInputModel inputModel)
    {
        var token = await _authService.Login(inputModel);
        return OkResponse(token);
    }

    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register Account")]
    [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterInputModel inputModel)
    {
        var registerUser = await _authService.Register(inputModel);
        return OkResponse(registerUser);
    }
}