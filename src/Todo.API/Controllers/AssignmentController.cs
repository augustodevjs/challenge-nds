using Microsoft.AspNetCore.Mvc;
using Todo.Application.Contracts;
using Todo.Application.DTO.Assignment;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Application.Contracts.Services;

namespace Todo.API.Controllers;

[Authorize]
[Route("Assignment")]
public class AssignmentController : MainController
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentController(
        INotificator notificador,
        IAssignmentService assignmentService) : base(notificador)
    {
        _assignmentService = assignmentService;
    }
    
    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get a to-do")]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetById(Guid id)
    {
        var getAssignment = await _assignmentService.GetById(id);
        return CustomResponse(getAssignment);
    }

    [HttpPost]
    [SwaggerOperation("Add a new to-do")]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create([FromBody] AddAssignmentDto addAssignmentDto)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);
        
        var createAssignment = await _assignmentService.Create(addAssignmentDto);
        return CustomResponse(createAssignment);
    }
    
    [HttpDelete("{id:guid}")]
    [SwaggerOperation("Delete a task")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _assignmentService.Delete(id);
        return CustomResponse();
    }
}

