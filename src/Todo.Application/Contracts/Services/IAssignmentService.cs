using Todo.Application.DTO.Assignment;

namespace Todo.Application.Contracts.Services;

public interface IAssignmentService
{
    Task<AssignmentDto?> GetById(Guid id);
    Task<AssignmentDto?> Create(AddAssignmentDto addAssignmentDto);
    Task Delete(Guid id);
}