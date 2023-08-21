using Microsoft.AspNetCore.Mvc;
using Todo.Application.Contracts;
using Todo.Application.DTO.Paged;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Application.DTO.AssignmentList;

namespace Todo.API.Controllers;

[Authorize]
[Route("AssignmentList")]
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
    [ProducesResponseType(typeof(PagedDto<AssignmentListDto>), StatusCodes.Status200OK)]
    public async Task<PagedDto<AssignmentListDto>> Search([FromQuery] AssignmentListSearchDto search)
    {
        return await _assignmentListService.Search(search);
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get a to-do list")]
    [ProducesResponseType(typeof(AssignmentListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetById(Guid id)
    {
        var getAssignmentList = await _assignmentListService.GetById(id);
        return CustomResponse(getAssignmentList);
    }
    
    [HttpPost]
    [SwaggerOperation(Summary = "Add a new to-do list")]
    [ProducesResponseType(typeof(AssignmentListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create([FromBody] AddAssignmentListDto addAssignmentListDto)
    {
        var createAssignmentList = await _assignmentListService.Create(addAssignmentListDto);
        return CustomResponse(createAssignmentList);
    }
    
    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update a to-do list")]
    [ProducesResponseType(typeof(AssignmentListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateAssignmentListDto updateAssignmentListDto)
    {
        var updateAssignmentList = await _assignmentListService.Update(id, updateAssignmentListDto);
        return CustomResponse(updateAssignmentList);
    }
}