using Todo.Application.DTO.Paged;
using Todo.Application.DTO.Assignment;

namespace Todo.Application.Contracts.Services;

public interface IAssignmentService
{
    Task<PagedDto<AssignmentDto>> Search(AssignmentSearchDto search);
    Task<AssignmentDto?> GetById(string id);
    Task<AssignmentDto?> Create(AddAssignmentDto addAssignmentDto);
    Task<AssignmentDto?> Update(string id, UpdateAssignmentDto updateAssignmentDto);
    Task Delete(string id);
    Task<AssignmentDto?> MarkConcluded(string id);
    Task<AssignmentDto?> MarkDesconcluded(string id);
}