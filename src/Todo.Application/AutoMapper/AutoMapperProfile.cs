using AutoMapper;
using Todo.Domain.Models;
using Todo.Application.DTO.Auth;
using Todo.Application.DTO.AssignmentList;

namespace Todo.Application.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<LoginDto, User>();
        CreateMap<RegisterDto, User>();
        CreateMap<UserDto, RegisterDto>();
        CreateMap<UpdateAssignmentListDto, AssignmentList>();
        CreateMap<AddAssignmentListDto, AssignmentList>();
        
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<AssignmentList, AssignmentListDto>().ReverseMap();
        CreateMap<UpdateAssignmentListDto, AssignmentListDto>().ReverseMap();
    }
}