using AutoMapper;
using Todo.Domain.Models;
using Todo.Services.DTO;

namespace Todo.API.Configuration;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<User, UserDTO>().ReverseMap();
    }
}