using AutoMapper;
using Todo.Domain.Filter;
using Todo.Domain.Models;
using Todo.Application.DTO.V1.ViewModel;
using Todo.Application.DTO.V1.InputModel;

namespace Todo.Application.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        #region Auth

        CreateMap<User, UserViewModel>().ReverseMap();
        CreateMap<LoginInputModel, User>().ReverseMap();
        CreateMap<RegisterInputModel, User>().ReverseMap();
        CreateMap<UserViewModel, RegisterInputModel>().ReverseMap();

        #endregion

        #region Assignment

        CreateMap<Assignment, AssignmentViewModel>().ReverseMap();
        CreateMap<AddAssignmentInputModel, Assignment>().ReverseMap();
        CreateMap<Assignment, UpdateAssignmentInputModel>().ReverseMap();
        CreateMap<AssignmentSearchInputModel, AssignmentFilter>().ReverseMap();
        CreateMap<UpdateAssignmentInputModel, AssignmentViewModel>().ReverseMap();

        #endregion

        #region AssignmentList

        CreateMap<AssignmentList, AssignmentListViewModel>().ReverseMap();
        CreateMap<AddAssignmentListInputModel, AssignmentList>().ReverseMap();
        CreateMap<UpdateAssignmentListInputModel, AssignmentList>().ReverseMap();

        #endregion
    }
}