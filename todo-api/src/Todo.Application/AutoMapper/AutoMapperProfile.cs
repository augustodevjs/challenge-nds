using AutoMapper;
using Todo.Domain.Filter;
using Todo.Domain.Models;
using Todo.Application.DTO.V1.Assignment;
using Todo.Application.DTO.V1.AssignmentList;
using Todo.Application.DTO.V1.Auth;

namespace Todo.Application.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        #region Auth

        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<LoginDto, User>().ReverseMap();
        CreateMap<RegisterDto, User>().ReverseMap();
        CreateMap<UserDto, RegisterDto>().ReverseMap();

        #endregion

        #region Assignment

        CreateMap<Assignment, AssignmentDto>().ReverseMap();
        CreateMap<AddAssignmentDto, Assignment>().ReverseMap();
        CreateMap<Assignment, UpdateAssignmentDto>().ReverseMap();
        CreateMap<UpdateAssignmentDto, AssignmentDto>().ReverseMap();
        CreateMap<AssignmentSearchDto, AssignmentFilter>().ReverseMap();

        #endregion

        #region AssignmentList

        CreateMap<AssignmentList, AssignmentListDto>().ReverseMap();
        CreateMap<AddAssignmentListDto, AssignmentList>().ReverseMap();
        CreateMap<UpdateAssignmentListDto, AssignmentList>().ReverseMap();

        #endregion
    }
}