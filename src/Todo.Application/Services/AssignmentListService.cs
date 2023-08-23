using AutoMapper;
using Todo.Domain.Models;
using System.Security.Claims;
using Todo.Domain.Validators;
using Microsoft.AspNetCore.Http;
using Todo.Application.DTO.Paged;
using Todo.Application.Contracts;
using Todo.Domain.Contracts.Repository;
using Todo.Application.DTO.AssignmentList;
using Todo.Application.Contracts.Services;

namespace Todo.Application.Services;

public class AssignmentListService : BaseService, IAssignmentListService
{
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAssignmentListRepository _assignmentListRepository;

    public AssignmentListService(
        IMapper mapper,
        INotificator notificator,
        IHttpContextAccessor httpContextAccessor,
        IAssignmentListRepository assignmentListRepository) : base(notificator)
    {
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _assignmentListRepository = assignmentListRepository;
    }
    
    public async Task<PagedDto<AssignmentListDto>> Search(AssignmentListSearchDto search)
    {
        var result = await _assignmentListRepository
            .Search(GetUserId(), search.Name, search.Description, search.PerPage, search.Page);

        return new PagedDto<AssignmentListDto>
        {
            Items = _mapper.Map<List<AssignmentListDto>>(result.Items),
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

        if (getAssignmentList == null)
        {
            Notify("Não foi possível encontrar a lista de tarefa correspondente.");
            return null;
        }

        return _mapper.Map<AssignmentListDto>(getAssignmentList);
    }
    
    public async Task<AssignmentListDto?> Create(AddAssignmentListDto addAssignmentListDto)
    {
        var assignmentList = _mapper.Map<AssignmentList>(addAssignmentListDto);
        assignmentList.UserId = Guid.Parse(GetUserId());

        if (!ExecuteValidation(new AssignmentListValidator(), assignmentList)) return null;

        await _assignmentListRepository.Create(assignmentList);

        return _mapper.Map<AssignmentListDto>(assignmentList);
    }

    public async Task<AssignmentListDto?> Update(string id, UpdateAssignmentListDto updateAssignmentListDto)
    {
        if (id != updateAssignmentListDto.Id || !Guid.TryParse(id, out _))
        {
            Notify("O id informado é inválido");
            return null;
        }

        var assignmentList = await _assignmentListRepository.GetById(id, GetUserId());

        if (assignmentList == null)
        {
            Notify("Não foi possível encontrar a lista de tarefa correspondente.");
            return null;
        }

        _mapper.Map(updateAssignmentListDto, assignmentList);

        if (!ExecuteValidation(new AssignmentListValidator(), assignmentList)) return null;

        await _assignmentListRepository.Update(assignmentList);

        return _mapper.Map<AssignmentListDto>(assignmentList);
    }

    public async Task Delete(string id)
    {
        if (!IsValidGuid(id)) return;
        
        var assignmentList = await _assignmentListRepository.GetById(id, GetUserId());

        if (assignmentList == null)
        {
            Notify("Não foi possível encontra a lista de tarefa correspondente.");
            return;
        }

        if (assignmentList.Assignments.Any(a => !a.Concluded))
        {
            Notify("Não é possível excluir lista com tarefas não concluídas!");
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
            Notify("O id informado é inválido");
            return false;
        }

        return true;
    }
}