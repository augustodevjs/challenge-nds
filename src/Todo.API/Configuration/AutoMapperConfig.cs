using AutoMapper;
using Todo.Services.DTO;
using Todo.Domain.Models;

namespace Todo.API.Configuration;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<LoginDTO, User>();
        CreateMap<UserDTO, RegisterDTO>();
        CreateMap<User, UserDTO>().ReverseMap();
    }
}