using AutoMapper;
using System.Text;
using Todo.Domain.Models;
using System.Security.Claims;
using Todo.Application.DTO.Auth;
using Todo.Application.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Todo.Domain.Contracts.Repository;
using Microsoft.Extensions.Configuration;

namespace Todo.Application.Services;

public class AuthService : BaseService, IAuthService
{
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(
        IMapper mapper,
        INotificator notificator,
        IConfiguration configuration,
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher
    ) : base(notificator)
    {
        _mapper = mapper;
        _configuration = configuration;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<TokenDto?> Login(LoginDto loginDto)
    {
        var user = await _userRepository.GetByEmail(loginDto.Email);

        if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password) !=
            PasswordVerificationResult.Success)
        {
            Notify("Usuário ou senha estão incorretos.");
            return null;
        }
        
        return GenerateToken(user);
    }

    public async Task<UserDto?> Register(RegisterDto registerDto)
    {
        var user = _mapper.Map<User>(registerDto);
        
        var getUser = await _userRepository.GetByEmail(registerDto.Email);
        
        if (getUser != null)
        {
            Notify("Já existe um usuário cadastrado com o email informado.");
            return null;
        }
        
        user.Password = _passwordHasher.HashPassword(user, registerDto.Password);
        
        await _userRepository.Create(user);

        return _mapper.Map<UserDto>(user);
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
            accessToken = encodedToken,
            expiresIn = TimeSpan.FromHours(int.Parse(_configuration["AppSettings:ExpirationHours"] ?? string.Empty))
                .TotalSeconds
        };
    }
}