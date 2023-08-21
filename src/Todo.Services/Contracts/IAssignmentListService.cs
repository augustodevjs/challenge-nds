using Todo.Services.DTO.AssignmentList;
using Todo.Services.DTO.Paged;

namespace Todo.Services.Contracts;

public interface IAssignmentListService
{
    Task<AssignmentListDto?> GetById(Guid? id);
    Task<PagedDto<AssignmentListDto>> Search(AssignmentListSearchDto search);
    Task<AssignmentListDto?> Create(AddAssignmentListDto addAssignmentListDto);
    Task<AssignmentListDto?> Update(Guid id ,UpdateAssignmentListDto updateAssignmentListDto);
}