using Todo.Services.DTO;
using Todo.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Todo.Services.Interfaces;

namespace Todo.API.Controllers;

[Route("api/v1")]
public class AuthController : MainController
{
    private readonly IAuthService _authService;
    private readonly IConfiguration _configuration;

    public AuthController(
        IAuthService authService,
        INotificator notificador,
        IConfiguration configuration
    ) : base(notificador)
    {
        _authService = authService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDTO loginDto)
    {
        var userToken = await _authService.Login(loginDto);
        var expireToken = TimeSpan.FromHours(int.Parse(_configuration["AppSettings:ExpirationHours"] ?? string.Empty))
            .TotalSeconds;

        return CustomResponse(new
        {
            Token = userToken,
            ExpiresIn = expireToken
        });
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