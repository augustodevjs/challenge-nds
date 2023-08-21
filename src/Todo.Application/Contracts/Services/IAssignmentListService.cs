using Todo.Application.DTO.AssignmentList;
using Todo.Application.DTO.Paged;

namespace Todo.Application.Contracts;

public interface IAssignmentListService
{
    Task<AssignmentListDto?> GetById(Guid? id);
    Task<PagedDto<AssignmentListDto>> Search(AssignmentListSearchDto search);
    Task<AssignmentListDto?> Create(AddAssignmentListDto addAssignmentListDto);
    Task<AssignmentListDto?> Update(Guid id ,UpdateAssignmentListDto updateAssignmentListDto);
}