using Todo.Application.DTO.V1.InputModel;
using Todo.Application.DTO.V1.ViewModel;

namespace Todo.Application.Contracts.Services;

public interface IAssignmentListService
{
    Task<PagedViewModel<AssignmentListViewModel>> Search(AssignmentListSearchInputModel inputModel);
    Task<PagedViewModel<AssignmentViewModel>?> SearchAssignments(int id, AssignmentSearchInputModel inputModel);
    Task<AssignmentListViewModel?> GetById(int? id);
    Task<AssignmentListViewModel?> Create(AddAssignmentListInputModel inputModel);
    Task<AssignmentListViewModel?> Update(int id ,UpdateAssignmentListInputModel inputModel);
    Task Delete(int id);
}