﻿using Microsoft.AspNetCore.Mvc;
using Todo.Application.Notifications;
using Todo.Application.DTO.V1.ViewModel;
using Todo.Application.DTO.V1.InputModel;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Application.Contracts.Services;

namespace Todo.API.Controllers;

[Authorize]
[Route("assignment")]
public class AssignmentController : MainController
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentController(
        INotificator notificador,
        IAssignmentService assignmentService
    ) : base(notificador)
    {
        _assignmentService = assignmentService;
    }

    [HttpGet]
    [SwaggerOperation("Search tasks")]
    [ProducesResponseType(typeof(PagedViewModel<AssignmentViewModel>), StatusCodes.Status200OK)]
    public async Task<PagedViewModel<AssignmentViewModel>> Search([FromQuery] AssignmentSearchInputModel inputModel)
    {
        return await _assignmentService.Search(inputModel);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get a to-do")]
    [ProducesResponseType(typeof(AssignmentViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var getAssignment = await _assignmentService.GetById(id);
        return OkResponse(getAssignment);
    }

    [HttpPost]
    [SwaggerOperation("Add a new to-do")]
    [ProducesResponseType(typeof(AssignmentViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] AddAssignmentInputModel inputModel)
    {
        var createAssignment = await _assignmentService.Create(inputModel);
        return CreatedResponse("", createAssignment);
    }

    [HttpPut("{id}")]
    [SwaggerOperation("Update a task")]
    [ProducesResponseType(typeof(AssignmentViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAssignmentInputModel inputModel)
    {
        var updateAssignment = await _assignmentService.Update(id, inputModel);
        return OkResponse(updateAssignment);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation("Delete a task")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _assignmentService.Delete(id);
        return NoContentResponse();
    }

    [HttpPatch("{id}/conclude")]
    [SwaggerOperation("Conclud a task")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Conclude(int id)
    {
        await _assignmentService.MarkConcluded(id);
        return NoContentResponse();
    }

    [HttpPatch("{id}/unconclude")]
    [SwaggerOperation("Desconclud a task")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Unconclude(int id)
    { 
        await _assignmentService.MarkDesconcluded(id);
        return NoContentResponse();
    }
}