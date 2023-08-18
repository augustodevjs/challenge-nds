using AutoMapper;
using Todo.Domain.Models;
using Todo.Services.DTO.AssignmentList;
using Todo.Services.DTO.Auth;

namespace Todo.API.Configuration;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<LoginDto, User>();
        CreateMap<UserDto, RegisterDto>();
        CreateMap<User, UserDto>().ReverseMap();
    }
}