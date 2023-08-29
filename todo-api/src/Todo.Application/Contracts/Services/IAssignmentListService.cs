using Todo.Application.DTO.V1.Assignment;
using Todo.Application.DTO.V1.AssignmentList;
using Todo.Application.DTO.V1.Paged;

namespace Todo.Application.Contracts.Services;

public interface IAssignmentListService
{
    Task<PagedDto<AssignmentListDto>> Search(AssignmentListSearchDto search);
    Task<PagedDto<AssignmentDto>?> SearchAssignments(string id, AssignmentSearchDto search);
    Task<AssignmentListDto?> GetById(string? id);
    Task<AssignmentListDto?> Create(AddAssignmentListDto addAssignmentListDto);
    Task<AssignmentListDto?> Update(string id ,UpdateAssignmentListDto updateAssignmentListDto);
    Task Delete(string id);
}