using AutoMapper;
using Todo.Domain.Models;
using System.Security.Claims;
using Todo.Domain.Validators;
using Microsoft.AspNetCore.Http;
using Todo.Application.DTO.Paged;
using Todo.Application.Contracts;
using Todo.Application.Contracts.Services;
using Todo.Domain.Contracts.Repository;
using Todo.Application.DTO.AssignmentList;

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
        IAssignmentListRepository assignmentListRepository
    ) : base(notificator)
    {
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _assignmentListRepository = assignmentListRepository;
    }

    public async Task<AssignmentListDto?> GetById(Guid? id)
    {
        var getUser = await _assignmentListRepository.GetById(id);

        if (getUser == null)
        {
            Notify("O ID fornecido é inválido. Não foi possível encontrar o usuário correspondente.");
            return null;
        }

        return _mapper.Map<AssignmentListDto>(getUser);
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

    public async Task<AssignmentListDto?> Create(AddAssignmentListDto addAssignmentListDto)
    {
        var assignmentList = _mapper.Map<AssignmentList>(addAssignmentListDto);
        assignmentList.UserId = GetUserId();

        if (!ExecuteValidation(new AssignmentListValidator(), assignmentList)) return null;

        await _assignmentListRepository.Create(assignmentList);

        return _mapper.Map<AssignmentListDto>(assignmentList);
    }

    public async Task<AssignmentListDto?> Update(Guid id, UpdateAssignmentListDto updateAssignmentListDto)
    {
        if (id != updateAssignmentListDto.Id)
        {
            Notify("O id informado é inválido");
            return null;
        }

        var getAssignmentList = await _assignmentListRepository.GetById(id);

        if (getAssignmentList == null)
        {
            Notify("O ID fornecido é inválido. Não foi possível encontrar o usuário correspondente.");
            return null;
        }

        _mapper.Map(updateAssignmentListDto, getAssignmentList);

        if (!ExecuteValidation(new AssignmentListValidator(), getAssignmentList)) return null;

        await _assignmentListRepository.Update(getAssignmentList);

        return _mapper.Map<AssignmentListDto>(updateAssignmentListDto);
    }

    private Guid GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userId == null ? Guid.Empty : Guid.Parse(userId);
    }
}