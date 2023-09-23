using AutoMapper;
using Todo.Core.Utils;
using Todo.Domain.Filter;
using Todo.Domain.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Todo.Application.Notifications;
using Todo.Domain.Contracts.Repository;
using Todo.Application.DTO.V1.ViewModel;
using Todo.Application.DTO.V1.InputModel;
using Todo.Application.Contracts.Services;

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

    public async Task<PagedViewModel<AssignmentListViewModel>> Search(AssignmentListSearchInputModel inputModel)
    {
        var result = await _assignmentListRepository
            .Search(GetUserId(), inputModel.Name, inputModel.Description, inputModel.PerPage, inputModel.Page);

        return new PagedViewModel<AssignmentListViewModel>
        {
            Items = Mapper.Map<List<AssignmentListViewModel>>(result.Items),
            Total = result.Total,
            Page = result.Page,
            PerPage = result.PerPage,
            PageCount = result.PageCount
        };
    }

    public async Task<PagedViewModel<AssignmentViewModel>?> SearchAssignments(string id, AssignmentSearchInputModel inputModel)
    {
        if (!GuidUtils.isValidGuid(id))
        {
            Notificator.Handle("O id informado é inválido");
            return null;
        }

        var filter = Mapper.Map<AssignmentFilter>(inputModel);
        var result = await _assignmentRepository
            .Search(GetUserId(), filter, inputModel.PerPage, inputModel.Page, id);

        return new PagedViewModel<AssignmentViewModel>
        {
            Items = Mapper.Map<List<AssignmentViewModel>>(result.Items),
            Total = result.Total,
            Page = result.Page,
            PerPage = result.PerPage,
            PageCount = result.PageCount
        };
    }

    public async Task<AssignmentListViewModel?> GetById(string? id)
    {
        if (!GuidUtils.isValidGuid(id))
        {
            Notificator.Handle("O id informado é inválido");
            return null;
        }

        var getAssignmentList = await _assignmentListRepository.GetById(id, GetUserId());

        if (getAssignmentList != null) return Mapper.Map<AssignmentListViewModel>(getAssignmentList);

        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<AssignmentListViewModel?> Create(AddAssignmentListInputModel inputModel)
    {
        var assignmentList = Mapper.Map<AssignmentList>(inputModel);
        assignmentList.UserId = Guid.Parse(GetUserId());

        if (!inputModel.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
            return null;
        }

        await _assignmentListRepository.Create(assignmentList);

        return Mapper.Map<AssignmentListViewModel>(assignmentList);
    }

    public async Task<AssignmentListViewModel?> Update(string id, UpdateAssignmentListInputModel inputModel)
    {
        if (id != inputModel.Id || !Guid.TryParse(id, out _))
        {
            Notificator.Handle("O id informado é inválido.");
            return null;
        }
        
        if (!inputModel.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
            return null;
        }

        var assignmentList = await _assignmentListRepository.GetById(id, GetUserId());

        if (assignmentList == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        Mapper.Map(inputModel, assignmentList);

        await _assignmentListRepository.Update(assignmentList);

        return Mapper.Map<AssignmentListViewModel>(assignmentList);
    }

    public async Task Delete(string id)
    {
        if (!GuidUtils.isValidGuid(id))
        {
            Notificator.Handle("O id informado é inválido");
            return;
        }

        var assignmentList = await _assignmentListRepository.GetById(id, GetUserId());

        if (assignmentList == null)
        {
            Notificator.HandleNotFoundResource();
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
}