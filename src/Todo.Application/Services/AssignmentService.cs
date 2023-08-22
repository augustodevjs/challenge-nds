using AutoMapper;
using Todo.Domain.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Todo.Application.Contracts;
using Todo.Application.DTO.Assignment;
using Todo.Domain.Contracts.Repository;
using Todo.Application.Contracts.Services;

namespace Todo.Application.Services;

public class AssignmentService : BaseService, IAssignmentService
{
    private readonly IMapper _mapper;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
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

    public async Task<AssignmentDto?> GetById(Guid id)
    {
        var getAssignment = await _assignmentRepository.GetById(id);

        if (getAssignment == null)
        {
            Notify("O ID fornecido é inválido. Não foi possível encontrar a tarefa correspondente.");
            return null;
        }

        return _mapper.Map<AssignmentDto>(getAssignment);
    }

    public async Task<AssignmentDto?> Create(AddAssignmentDto addAssignmentDto)
    {
        var getAssignment = await _assignmentListRepository.GetById(addAssignmentDto.AssignmentListId);

        if (getAssignment == null)
        {
            Notify("O ID fornecido é inválido. Não foi possível encontrar essa lista de tarefas correspondente.");
            return null;
        }

        var assignment = _mapper.Map<Assignment>(addAssignmentDto);
        assignment.UserId = GetUserId();

        await _assignmentRepository.Create(assignment);

        return _mapper.Map<AssignmentDto>(assignment);
    }

    public async Task Delete(Guid id)
    {
        var getAssignment = await _assignmentRepository.GetById(id);

        if (getAssignment == null)
        {
            Notify("O ID fornecido é inválido. Não foi possível encontrar essa tarefa correspondente.");
            return;
        }

        await _assignmentRepository.Delete(getAssignment);
    }

    private Guid GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userId == null ? Guid.Empty : Guid.Parse(userId);
    }
}