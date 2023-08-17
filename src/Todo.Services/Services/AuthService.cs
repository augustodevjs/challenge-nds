using AutoMapper;
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
    private readonly IUserRepository _userRepository;

    public AuthService(
        IMapper mapper,
        INotificator notificator, 
        IUserRepository userRepository 
        ) : base(notificator)
    {
        _mapper = mapper;
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

    public async Task<User?> Login(LoginDTO loginDto)
    {
        var userMapper = _mapper.Map<User>(loginDto);

        if (!ExecutarValidacao(new LoginValidator(), userMapper)) return null; 
        
        var getUser = await _userRepository.GetByEmail(loginDto.Email);

        if (getUser == null || loginDto.Password != getUser.Password)
        {
            Notificar("Usuário ou senha estão incorretos.");
            return null;
        }

        return getUser;
    }
}