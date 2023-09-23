using Todo.Application.DTO.V1.ViewModel;
using Todo.Application.DTO.V1.InputModel;

namespace Todo.Application.Contracts.Services;

public interface IAssignmentService
{
    Task<PagedViewModel<AssignmentViewModel>> Search(AssignmentSearchInputModel inputModel);
    Task<AssignmentViewModel?> GetById(string id);
    Task<AssignmentViewModel?> Create(AddAssignmentInputModel inputModel);
    Task<AssignmentViewModel?> Update(string id, UpdateAssignmentInputModel inputModel);
    Task Delete(string id);
    Task MarkConcluded(string id);
    Task MarkDesconcluded(string id);
}