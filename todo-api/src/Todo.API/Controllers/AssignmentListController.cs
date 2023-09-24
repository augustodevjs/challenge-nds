using Todo.API.Responses;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.Notifications;
using Todo.Application.DTO.V1.ViewModel;
using Todo.Application.DTO.V1.InputModel;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Application.Contracts.Services;

namespace Todo.API.Controllers;

[Authorize]
[Route("assignmentList")]
public class AssignmentListController : MainController
{
    private readonly IAssignmentListService _assignmentListService;

    public AssignmentListController(
        INotificator notificador,
        IAssignmentListService assignmentListService
    ) : base(notificador)
    {
        _assignmentListService = assignmentListService;
    }

    [HttpGet]
    [SwaggerOperation("Search to-do lists")]
    [ProducesResponseType(typeof(PagedViewModel<AssignmentListViewModel>), StatusCodes.Status200OK)]
    public async Task<PagedViewModel<AssignmentListViewModel>> Search([FromQuery] AssignmentListSearchInputModel inputModel)
    {
        return await _assignmentListService.Search(inputModel);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get a to-do list")]
    [ProducesResponseType(typeof(AssignmentListViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var getAssignmentList = await _assignmentListService.GetById(id);
        return OkResponse(getAssignmentList);
    }

    [HttpGet("{id}/assignments")]
    [SwaggerOperation("Search for tasks in a to-do list")]
    [ProducesResponseType(typeof(IEnumerable<AssignmentViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAssignment(int id, [FromQuery] AssignmentSearchInputModel inputModel)
    {
        var getAssignment = await _assignmentListService.SearchAssignments(id, inputModel);
        return OkResponse(getAssignment);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Add a new to-do list")]
    [ProducesResponseType(typeof(AssignmentListViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] AddAssignmentListInputModel inputModel)
    {
        var createAssignmentList = await _assignmentListService.Create(inputModel);
        return CreatedResponse("", createAssignmentList);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a to-do list")]
    [ProducesResponseType(typeof(AssignmentListViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAssignmentListInputModel inputModel)
    {
        var updateAssignmentList = await _assignmentListService.Update(id, inputModel);
        return OkResponse(updateAssignmentList);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation("Delete a todo-list")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult) ,StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _assignmentListService.Delete(id);
        return NoContentResponse();
    }
}