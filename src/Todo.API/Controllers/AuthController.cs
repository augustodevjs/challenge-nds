using Microsoft.AspNetCore.Mvc;
using Todo.API.Token;
using Todo.API.ViewModels;
using Todo.Core.Interfaces;

namespace Todo.API.Controllers;

[Route("api")]
public class AuthController : MainController
{
    private readonly IConfiguration _configuration;
    private readonly ITokenGenerator _tokenGenerator;

    public AuthController(INotificator notificador, IConfiguration configuration, ITokenGenerator tokenGenerator) :
        base(notificador)
    {
        _configuration = configuration;
        _tokenGenerator = tokenGenerator;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login(LoginViewModel loginViewModel)
    {
        var tokenLogin = _configuration["Jwt:Login"];
        var tokenPassword = _configuration["Jwt:Password"];

        if (loginViewModel.Login == tokenLogin && loginViewModel.Password == tokenPassword)
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
}