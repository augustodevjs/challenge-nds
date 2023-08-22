using AutoMapper;
using Todo.Application.DTO.Assignment;
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
        CreateMap<AddAssignmentListDto, AssignmentList>();
        CreateMap<UpdateAssignmentListDto, AssignmentList>();
        CreateMap<Assignment, AssignmentDto>();
        
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<AddAssignmentDto, Assignment>();
        CreateMap<AssignmentList, AssignmentListDto>().ReverseMap();
        CreateMap<UpdateAssignmentListDto, AssignmentListDto>().ReverseMap();
    }
}