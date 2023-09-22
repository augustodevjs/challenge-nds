using AutoMapper;
using System.Text;
using Todo.Domain.Models;
using System.Security.Claims;
using Todo.Application.DTO.V1.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Todo.Application.Notifications;
using System.IdentityModel.Tokens.Jwt;
using Todo.Domain.Contracts.Repository;
using Microsoft.Extensions.Configuration;
using Todo.Application.Contracts.Services;

namespace Todo.Application.Services;

public class AuthService : BaseService, IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(
        IMapper mapper,
        INotificator notificator,
        IConfiguration configuration,
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher) : base(mapper, notificator)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<TokenDto?> Login(LoginDto loginDto)
    {
        if (!loginDto.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
            return null;
        }
        
        var user = await _userRepository.GetByEmail(loginDto.Email);

        if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password) !=
            PasswordVerificationResult.Success)
        {
            Notificator.Handle("Usuário ou senha estão incorretos.");
            return null;
        }

        return GenerateToken(user);
    }

    public async Task<UserDto?> Register(RegisterDto registerDto)
    {
        if (!registerDto.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
            return null;
        }
        
        var user = Mapper.Map<User>(registerDto);

        var getUser = await _userRepository.GetByEmail(registerDto.Email);

        if (getUser != null)
        {
            Notificator.Handle("Já existe um usuário cadastrado com o email informado.");
            return null;
        }

        user.Password = _passwordHasher.HashPassword(user, registerDto.Password);

        await _userRepository.Create(user);

        return Mapper.Map<UserDto>(user);
    }

    private TokenDto GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"] ?? string.Empty);

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Role, "User"),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            }),
            SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            Expires =
                DateTime.UtcNow.AddHours(int.Parse(_configuration["AppSettings:ExpirationHours"] ?? string.Empty)),
            Issuer = _configuration["AppSettings:Issuer"],
            Audience = _configuration["AppSettings:ValidOn"]
        });

        var encodedToken = tokenHandler.WriteToken(token);

        return new TokenDto
        {
            accessToken = encodedToken
        };
    }
}