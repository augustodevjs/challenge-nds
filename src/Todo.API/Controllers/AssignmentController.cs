using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Application.Contracts;
using Todo.Application.Contracts.Services;
using Todo.Application.DTO.Assignment;

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
}