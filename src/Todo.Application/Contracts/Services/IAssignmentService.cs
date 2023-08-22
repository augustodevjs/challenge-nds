using Todo.Application.DTO.Assignment;

namespace Todo.Application.Contracts.Services;

public interface IAssignmentService
{
    Task<AssignmentDto?> Create(AddAssignmentDto addAssignmentDto);
}