using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Todo.API.Token;
using Todo.API.ViewModels;
using Todo.Core.Interfaces;
using Todo.Services.DTO;
using Todo.Services.Interfaces;

namespace Todo.API.Controllers;

[Route("api")]
public class AuthController : MainController
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    private readonly ITokenGenerator _tokenGenerator;

    public AuthController(
        IMapper mapper,
        IUserService userService,
        INotificator notificador,
        IConfiguration configuration,
        ITokenGenerator tokenGenerator
    ) : base(notificador)
    {
        _mapper = mapper;
        _userService = userService;
        _configuration = configuration;
        _tokenGenerator = tokenGenerator;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login(LoginViewModel loginViewModel)
    {
        var tokenEmail = _configuration["Jwt:Email"];
        var tokenPassword = _configuration["Jwt:Password"];

        if (loginViewModel.Email == tokenEmail && loginViewModel.Password == tokenPassword)
        {
            return CustomResponse(new
            {
                Token = _tokenGenerator.GenerateToken(),
                TokenExpires = DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:HoursToExpire"]))
            });
        }

        NotificarErro("Usuário ou senha estão incorretas.");
        return CustomResponse(loginViewModel);
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult> Register(UserDTO userDto)
    {
        await _userService.Create(userDto);

        return CustomResponse(new
        {
            userDto.Name,   
            userDto.Email,
        });
    }
}