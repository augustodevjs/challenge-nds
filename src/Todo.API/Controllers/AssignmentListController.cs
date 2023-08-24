using Microsoft.AspNetCore.Mvc;
using Todo.Application.Contracts;
using Todo.Application.DTO.Paged;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Application.Contracts.Services;
using Todo.Application.DTO.Assignment;
using Todo.Application.DTO.AssignmentList;

namespace Todo.API.Controllers;

[Authorize]
[Route("assignmentList")]
public class AssignmentListController : MainController
{
    private readonly IAssignmentListService _assignmentListService;

    public AssignmentListController(
        INotificator notificador,
        IAssignmentListService assignmentListService) : base(notificador)
    {
        _assignmentListService = assignmentListService;
    }

    [HttpGet]
    [SwaggerOperation("Search to-do lists")]
    [ProducesResponseType(typeof(PagedDto<AssignmentListDto>), StatusCodes.Status200OK)]
    public async Task<PagedDto<AssignmentListDto>> Search([FromQuery] AssignmentListSearchDto search)
    {
        return await _assignmentListService.Search(search);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get a to-do list")]
    [ProducesResponseType(typeof(AssignmentListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(string id)
    {
        var getAssignmentList = await _assignmentListService.GetById(id);
        return CustomResponse(getAssignmentList);
    }
    
    [HttpGet("{id}/assignments")]
    [SwaggerOperation("Search for tasks in a to-do list")]
    [ProducesResponseType(typeof(IEnumerable<AssignmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAssignment(string id, [FromQuery] AssignmentSearchDto search)
    {
        var getAssignment = await _assignmentListService.SearchAssignments(id, search);
        return CustomResponse(getAssignment);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Add a new to-do list")]
    [ProducesResponseType(typeof(AssignmentListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Create([FromBody] AddAssignmentListDto addAssignmentListDto)
    {
        var createAssignmentList = await _assignmentListService.Create(addAssignmentListDto);
        return CustomResponse(createAssignmentList);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a to-do list")]
    [ProducesResponseType(typeof(AssignmentListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(string id, [FromBody] UpdateAssignmentListDto updateAssignmentListDto)
    {
        var updateAssignmentList = await _assignmentListService.Update(id, updateAssignmentListDto);
        return CustomResponse(updateAssignmentList);
    }
    
    [HttpDelete("{id}")]
    [SwaggerOperation("Delete a todo-list")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        await _assignmentListService.Delete(id);
        return CustomResponse();
    }
}