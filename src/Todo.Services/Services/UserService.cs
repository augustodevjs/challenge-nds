using AutoMapper;
using Todo.Core.Interfaces;
using Todo.Domain.Models;
using Todo.Domain.Validators;
using Todo.Infra.Interfaces;
using Todo.Services.DTO;
using Todo.Services.Interfaces;

namespace Todo.Services.Services;

public class UserService : BaseService, IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserService(
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

        var userExists = _userRepository.GetByEmail(userDto.Email);

        if (userExists.Result != null)
        {
            Notificar("Já existe um usuário cadastrado com o email informado.");
            return;
        }

        await _userRepository.Create(user);
    }
}