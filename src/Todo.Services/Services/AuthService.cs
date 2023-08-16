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
        var user = _mapper.Map<User>(userDto);

        if (!ExecutarValidacao(new UserValidator(), user)) return;

        var userExists = await _userRepository.GetByEmail(userDto.Email);

        if (userExists != null)
        {
            Notificar("Já existe um usuário cadastrado com o email informado.");
            return;
        }

        await _userRepository.Create(user);
    }

    public async Task<bool> Login(UserDTO userDto)
    {
        var user = _mapper.Map<User>(userDto);

        if (!ExecutarValidacao(new LoginValidator(), user)) return false; 
        
        var userByEmail = await _userRepository.GetByEmail(userDto.Email);

        if (userByEmail == null || userDto.Password != userByEmail.Password)
        {
            Notificar("Usuário ou senha estão incorretos.");
            return false;
        }

        return true;
    }
}