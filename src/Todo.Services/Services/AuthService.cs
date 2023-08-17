using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Todo.Core.Interfaces;
using Todo.Domain.Models;
using Todo.Domain.Validators;
using Todo.Infra.Interfaces;
using Todo.Services.DTO;
using Todo.Services.Interfaces;

namespace Todo.Services.Services;

public class AuthService : BaseService, IAuthService
{
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public AuthService(
        IMapper mapper,
        INotificator notificator,
        IConfiguration configuration,
        IUserRepository userRepository
    ) : base(notificator)
    {
        _mapper = mapper;
        _configuration = configuration;
        _userRepository = userRepository;
    }

    public async Task Create(UserDTO userDto)
    {
        var userMapper = _mapper.Map<User>(userDto);

        if (!ExecutarValidacao(new UserValidator(), userMapper)) return;

        var getUser = await _userRepository.GetByEmail(userDto.Email);

        if (getUser != null)
        {
            Notificar("Já existe um usuário cadastrado com o email informado.");
            return;
        }

        await _userRepository.Create(userMapper);
    }

    public async Task<string?> Login(LoginDTO loginDto)
    {
        var userMapper = _mapper.Map<User>(loginDto);

        if (!ExecutarValidacao(new LoginValidator(), userMapper)) return null;

        var user = await _userRepository.GetByEmail(loginDto.Email);

        if (user == null || loginDto.Password != user.Password)
        {
            Notificar("Usuário ou senha estão incorretos.");
            return null;
        }

        return GenerateToken(user);
    }

    private string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"] ?? string.Empty);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Role, "User"),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
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