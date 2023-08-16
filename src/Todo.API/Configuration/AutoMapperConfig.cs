using AutoMapper;
using Todo.API.ViewModels;
using Todo.Domain.Models;
using Todo.Services.DTO;

namespace Todo.API.Configuration;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<LoginViewModel, UserDTO>();
        CreateMap<User, UserDTO>().ReverseMap();
    }
}