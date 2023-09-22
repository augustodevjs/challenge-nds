using AutoMapper;
using Todo.Domain.Filter;
using Todo.Domain.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Todo.Application.DTO.V1.Paged;
using Todo.Application.Notifications;
using Todo.Domain.Contracts.Repository;
using Todo.Application.DTO.V1.Assignment;
using Todo.Application.Contracts.Services;
using Todo.Application.DTO.V1.AssignmentList;

namespace Todo.Application.Services;

public class AssignmentListService : BaseService, IAssignmentListService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IAssignmentListRepository _assignmentListRepository;

    public AssignmentListService(
        IMapper mapper,
        INotificator notificator,
        IHttpContextAccessor httpContextAccessor,
        IAssignmentRepository assignmentRepository,
        IAssignmentListRepository assignmentListRepository) : base(mapper, notificator)
    {
        _httpContextAccessor = httpContextAccessor;
        _assignmentRepository = assignmentRepository;
        _assignmentListRepository = assignmentListRepository;
    }

    public async Task<PagedDto<AssignmentListDto>> Search(AssignmentListSearchDto search)
    {
        var result = await _assignmentListRepository
            .Search(GetUserId(), search.Name, search.Description, search.PerPage, search.Page);

        return new PagedDto<AssignmentListDto>
        {
            Items = Mapper.Map<List<AssignmentListDto>>(result.Items),
            Total = result.Total,
            Page = result.Page,
            PerPage = result.PerPage,
            PageCount = result.PageCount
        };
    }

    public async Task<PagedDto<AssignmentDto>?> SearchAssignments(string id, AssignmentSearchDto search)
    {
        if (!IsValidGuid(id)) return null;

        var filter = Mapper.Map<AssignmentFilter>(search);
        var result = await _assignmentRepository
            .Search(GetUserId(), filter, search.PerPage, search.Page, id);

        return new PagedDto<AssignmentDto>
        {
            Items = Mapper.Map<List<AssignmentDto>>(result.Items),
            Total = result.Total,
            Page = result.Page,
            PerPage = result.PerPage,
            PageCount = result.PageCount
        };
    }

    public async Task<AssignmentListDto?> GetById(string? id)
    {
        if (!IsValidGuid(id)) return null;

        var getAssignmentList = await _assignmentListRepository.GetById(id, GetUserId());

        if (getAssignmentList != null) return Mapper.Map<AssignmentListDto>(getAssignmentList);

        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<AssignmentListDto?> Create(AddAssignmentListDto addAssignmentListDto)
    {
        var assignmentList = Mapper.Map<AssignmentList>(addAssignmentListDto);
        assignmentList.UserId = Guid.Parse(GetUserId());

        // if (!ExecuteValidation(new AssignmentListValidator(), assignmentList)) return null;

        await _assignmentListRepository.Create(assignmentList);

        return Mapper.Map<AssignmentListDto>(assignmentList);
    }

    public async Task<AssignmentListDto?> Update(string id, UpdateAssignmentListDto updateAssignmentListDto)
    {
        if (id != updateAssignmentListDto.Id || !Guid.TryParse(id, out _))
        {
            Notificator.Handle("O id informado é inválido.");
            return null;
        }

        var assignmentList = await _assignmentListRepository.GetById(id, GetUserId());

        if (assignmentList == null)
        {
            Notificator.Handle("Não foi possível encontrar a lista de tarefa correspondente.");
            return null;
        }

        Mapper.Map(updateAssignmentListDto, assignmentList);

        // if (!ExecuteValidation(new AssignmentListValidator(), assignmentList)) return null;

        await _assignmentListRepository.Update(assignmentList);

        return Mapper.Map<AssignmentListDto>(assignmentList);
    }

    public async Task Delete(string id)
    {
        if (!IsValidGuid(id)) return;

        var assignmentList = await _assignmentListRepository.GetById(id, GetUserId());

        if (assignmentList == null)
        {
            Notificator.Handle("Não foi possível encontrar a lista de tarefa correspondente.");
            return;
        }

        if (assignmentList.Assignments.Any(a => !a.Concluded))
        {
            Notificator.Handle("Não é possível excluir lista com tarefas não concluídas.");
            return;
        }

        await _assignmentListRepository.Delete(assignmentList);
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
            Notificator.Handle("O id informado é inválido.");
            return false;
        }

        return true;
    }
}