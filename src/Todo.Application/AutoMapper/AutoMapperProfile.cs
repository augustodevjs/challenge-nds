using AutoMapper;
using Todo.Domain.Models;
using Todo.Application.DTO.Auth;
using Todo.Application.DTO.Assignment;
using Todo.Application.DTO.AssignmentList;

namespace Todo.Application.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<LoginDto, User>();
        CreateMap<RegisterDto, User>();
        CreateMap<UserDto, RegisterDto>();
        CreateMap<Assignment, AssignmentDto>();
        CreateMap<AddAssignmentDto, Assignment>();
        CreateMap<UpdateAssignmentDto, AssignmentDto>();
        CreateMap<AddAssignmentListDto, AssignmentList>();
        CreateMap<UpdateAssignmentListDto, AssignmentList>();

        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<Assignment, UpdateAssignmentDto>().ReverseMap();
        CreateMap<AssignmentList, AssignmentListDto>().ReverseMap();
        CreateMap<UpdateAssignmentListDto, AssignmentListDto>().ReverseMap();
    }
}