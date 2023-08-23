using Todo.Application.DTO.Paged;
using Todo.Application.DTO.AssignmentList;

namespace Todo.Application.Contracts.Services;

public interface IAssignmentListService
{
    Task<PagedDto<AssignmentListDto>> Search(AssignmentListSearchDto search);
    Task<AssignmentListDto?> GetById(string? id);
    Task<AssignmentListDto?> Create(AddAssignmentListDto addAssignmentListDto);
    Task<AssignmentListDto?> Update(string id ,UpdateAssignmentListDto updateAssignmentListDto);
    Task Delete(string id);
}