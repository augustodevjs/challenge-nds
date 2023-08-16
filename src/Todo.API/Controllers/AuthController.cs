using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Todo.API.Token;
using Todo.API.ViewModels;
using Todo.Core.Interfaces;
using Todo.Services.DTO;
using Todo.Services.Interfaces;

namespace Todo.API.Controllers;

[Route("api/v1")]
public class AuthController : MainController
{
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;
    private readonly IConfiguration _configuration;
    private readonly ITokenGenerator _tokenGenerator;

    public AuthController(
        IMapper mapper,
        IAuthService authService,
        INotificator notificador,
        IConfiguration configuration,
        ITokenGenerator tokenGenerator
    ) : base(notificador)
    {
        _mapper = mapper;
        _authService = authService;
        _configuration = configuration;
        _tokenGenerator = tokenGenerator;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login(LoginViewModel loginViewModel)
    {
        var user = _mapper.Map<UserDTO>(loginViewModel);

        var isLogged = await _authService.Login(user);

        if (isLogged)
        {
            return CustomResponse(new
            {
                Token = _tokenGenerator.GenerateToken(),
                TokenExpires = DateTime.UtcNow
                    .AddHours(int.Parse(_configuration["Jwt:HoursToExpire"] ?? string.Empty))
            });
        }

        return CustomResponse(loginViewModel);
    }

    [HttpPost]
    [Route("register")]
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