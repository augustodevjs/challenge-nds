using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Application.Contracts;
using Todo.Application.Contracts.Services;
using Todo.Application.DTO.V1.Assignment;
using Todo.Application.DTO.V1.Paged;

namespace Todo.API.V1.Controllers;

[Authorize]
[Route("assignment")]
public class AssignmentController : MainController
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentController(
        INotificator notificador,
        IAssignmentService assignmentService) : base(notificador)
    {
        _assignmentService = assignmentService;
    }
    
    [HttpGet]
    [SwaggerOperation("Search tasks")]
    [ProducesResponseType(typeof(PagedDto<AssignmentDto>), StatusCodes.Status200OK)]
    public async Task<PagedDto<AssignmentDto>> Search([FromQuery] AssignmentSearchDto search)
    {
        return await _assignmentService.Search(search);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get a to-do")]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(string id)
    {
        var getAssignment = await _assignmentService.GetById(id);
        return CustomResponse(getAssignment);
    }

    [HttpPost]
    [SwaggerOperation("Add a new to-do")]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Create([FromBody] AddAssignmentDto addAssignmentDto)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var createAssignment = await _assignmentService.Create(addAssignmentDto);
        return CustomResponse(createAssignment);
    }

    [HttpPut("{id}")]
    [SwaggerOperation("Update a task")]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateAssignmentDto updateAssignmentDto)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);
        
        var updateAssignment = await _assignmentService.Update(id, updateAssignmentDto);
        return CustomResponse(updateAssignment);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation("Delete a task")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        await _assignmentService.Delete(id);
        return CustomResponse();
    }
    
    [HttpPatch("{id}/conclude")]
    [SwaggerOperation("Conclud a task")]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Conclude(string id)
    {
        var concludedAssignment = await _assignmentService.MarkConcluded(id);
        return CustomResponse(concludedAssignment);
    }
    
    [HttpPatch("{id}/unconclude")]
    [SwaggerOperation("Desconclud a task")]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Unconclude(string id)
    {
        var desconcludedAssignment = await _assignmentService.MarkDesconcluded(id);
        return CustomResponse(desconcludedAssignment);
    }
}