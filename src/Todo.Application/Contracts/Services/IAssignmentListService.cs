using Todo.Application.DTO.AssignmentList;

namespace Todo.Application.Contracts;

public interface IAssignmentListService
{
    Task<AssignmentListDto?> GetById(Guid? id);
    Task<AssignmentListDto?> Create(AddAssignmentListDto addAssignmentListDto);
    Task<AssignmentListDto?> Update(Guid id ,UpdateAssignmentListDto updateAssignmentListDto);
}