using AutoMapper;
using Todo.Domain.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Todo.Application.Contracts;
using Todo.Application.DTO.Assignment;
using Todo.Domain.Contracts.Repository;
using Todo.Application.Contracts.Services;
using Todo.Application.DTO.Paged;
using Todo.Domain.Filter;

namespace Todo.Application.Services;

public class AssignmentService : BaseService, IAssignmentService
{
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IAssignmentListRepository _assignmentListRepository;

    public AssignmentService(
        IMapper mapper,
        INotificator notificator,
        IAssignmentRepository assignmentRepository,
        IAssignmentListRepository assignmentListRepository,
        IHttpContextAccessor httpContextAccessor) : base(notificator)
    {
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _assignmentRepository = assignmentRepository;
        _assignmentListRepository = assignmentListRepository;
    }

    public async Task<PagedDto<AssignmentDto>> Search(AssignmentSearchDto search)
    {
        var filter = _mapper.Map<AssignmentFilter>(search);
        var result = await _assignmentRepository.Search(GetUserId(), filter, search.PerPage, search.Page);

        return new PagedDto<AssignmentDto>
        {
            Items = _mapper.Map<List<AssignmentDto>>(result.Items),
            Total = result.Total,
            Page = result.Page,
            PerPage = result.PerPage,
            PageCount = result.PageCount
        };
    }

    public async Task<AssignmentDto?> GetById(string id)
    {
        if (!IsValidGuid(id)) return null;

        var getAssignment = await _assignmentRepository.GetById(id, GetUserId());

        if (getAssignment == null)
        {
            Notify("Não foi possível encontrar a tarefa correspondente.");
            return null;
        }

        return _mapper.Map<AssignmentDto>(getAssignment);
    }

    public async Task<AssignmentDto?> Create(AddAssignmentDto addAssignmentDto)
    {
        if (!IsValidGuid(addAssignmentDto.AssignmentListId)) return null;

        var getAssignment = await _assignmentListRepository.GetById(Guid.Parse(addAssignmentDto.AssignmentListId));

        if (getAssignment == null)
        {
            Notify("Não foi possível encontrar a lista de tarefas correspondente.");
            return null;
        }

        var assignment = _mapper.Map<Assignment>(addAssignmentDto);
        assignment.UserId = Guid.Parse(GetUserId());

        await _assignmentRepository.Create(assignment);

        return _mapper.Map<AssignmentDto>(assignment);
    }

    public async Task<AssignmentDto?> Update(string id, UpdateAssignmentDto updateAssignmentDto)
    {
        bool isIdInvalid = !Guid.TryParse(id, out _) || !Guid.TryParse(updateAssignmentDto.Id, out _);
        
        if (id != updateAssignmentDto.Id || isIdInvalid)
        {
            Notify("O id informado é inválido");
            return null;
        }

        var getAssignment = await _assignmentRepository.GetById(id, GetUserId());

        if (getAssignment == null)
        {
            Notify("Não foi possível encontrar a tarefa correspondente.");
            return null;
        }

        _mapper.Map(updateAssignmentDto, getAssignment);

        await _assignmentRepository.Update(getAssignment);

        return _mapper.Map<AssignmentDto>(updateAssignmentDto);
    }

    public async Task Delete(string id)
    {
        if (!IsValidGuid(id)) return;

        var getAssignment = await _assignmentRepository.GetById(id, GetUserId());

        if (getAssignment == null)
        {
            Notify("Não foi possível encontrar a tarefa correspondente.");
            return;
        }

        await _assignmentRepository.Delete(getAssignment);
    }

    public async Task<AssignmentDto?> MarkConcluded(string id)
    {
        if (!IsValidGuid(id)) return null;

        var assignment = await _assignmentRepository.GetById(id, GetUserId());

        if (assignment == null)
        {
            Notify("Não foi possível encontrar a tarefa correspondente.");
            return null;
        }

        assignment.SetConcluded();

        await _assignmentRepository.Update(assignment);
        return _mapper.Map<AssignmentDto>(assignment);
    }

    public async Task<AssignmentDto?> MarkDesconcluded(string id)
    {
        if (!IsValidGuid(id)) return null;

        var assignment = await _assignmentRepository.GetById(id, GetUserId());

        if (assignment == null)
        {
            Notify("Não foi possível encontrar a tarefa correspondente.");
            return null;
        }

        assignment.SetUnconcluded();

        await _assignmentRepository.Update(assignment);
        return _mapper.Map<AssignmentDto>(assignment);
    }

    private string GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userId ?? string.Empty;
    }

    private bool IsValidGuid(string? id)
    {
        if (!Guid.TryParse(id, out _))
        {
            Notify("O id informado é inválido");
            return false;
        }

        return true;
    }
}