using Todo.Services.DTO.AssignmentList;

namespace Todo.Services.Contracts;

public interface IAssignmentListService
{
    Task<AssignmentListDto?> GetById(string? id);
    Task<AssignmentListDto?> Create(AddAssignmentListDto addAssignmentListDto);
    Task<AssignmentListDto?> Update(string id ,UpdateAssignmentListDto updateAssignmentListDto);
}