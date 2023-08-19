using AutoMapper;
using Todo.Domain.Models;
using Todo.Services.DTO.Auth;
using Todo.Services.DTO.AssignmentList;

namespace Todo.API.Configuration;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<LoginDto, User>();
        CreateMap<RegisterDto, User>();
        CreateMap<UserDto, RegisterDto>();
        CreateMap<UpdateAssignmentListDto, AssignmentList>();
        CreateMap<UpdateAssignmentListDto, AssignmentListDto>().ReverseMap();
        
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<AssignmentList, AssignmentListDto>().ReverseMap();
        CreateMap<AssignmentList, AssignmentListDto>().ReverseMap();
    }
}