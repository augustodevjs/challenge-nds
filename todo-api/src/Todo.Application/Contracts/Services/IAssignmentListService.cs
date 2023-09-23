using Todo.Application.DTO.V1.InputModel;
using Todo.Application.DTO.V1.ViewModel;

namespace Todo.Application.Contracts.Services;

public interface IAssignmentListService
{
    Task<PagedViewModel<AssignmentListViewModel>> Search(AssignmentListSearchInputModel inputModel);
    Task<PagedViewModel<AssignmentViewModel>?> SearchAssignments(string id, AssignmentSearchInputModel inputModel);
    Task<AssignmentListViewModel?> GetById(string? id);
    Task<AssignmentListViewModel?> Create(AddAssignmentListInputModel inputModel);
    Task<AssignmentListViewModel?> Update(string id ,UpdateAssignmentListInputModel inputModel);
    Task Delete(string id);
}