using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Todo.Core.Interfaces;
using Todo.Services.DTO;
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
        var userLogged = await _authService.Login(loginDto);

        if (userLogged != null)
        {
            return CustomResponse(new
            {
                Token = GenerateToken(userLogged.Name),
                ExpiresIn = TimeSpan.FromHours(int.Parse(_configuration["AppSettings:ExpirationHours"] ?? string.Empty)).TotalSeconds
            });
        }

        return CustomResponse(loginDto);
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

    private string GenerateToken(string name)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"] ?? string.Empty);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Name, name),
                new(ClaimTypes.Role, "User")
            }),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Expires =
                DateTime.UtcNow.AddHours(int.Parse(_configuration["AppSettings:ExpirationHours"] ?? string.Empty)),
            Issuer = _configuration["AppSettings:Issuer"],
            Audience = _configuration["AppSettings:ValidOn"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}