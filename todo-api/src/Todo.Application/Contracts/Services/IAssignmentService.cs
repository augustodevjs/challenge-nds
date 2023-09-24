using Todo.Application.DTO.V1.ViewModel;
using Todo.Application.DTO.V1.InputModel;

namespace Todo.Application.Contracts.Services;

public interface IAssignmentService
{
    Task<PagedViewModel<AssignmentViewModel>> Search(AssignmentSearchInputModel inputModel);
    Task<AssignmentViewModel?> GetById(int id);
    Task<AssignmentViewModel?> Create(AddAssignmentInputModel inputModel);
    Task<AssignmentViewModel?> Update(int id, UpdateAssignmentInputModel inputModel);
    Task Delete(int id);
    Task MarkConcluded(int id);
    Task MarkDesconcluded(int id);
}