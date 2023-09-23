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

public class AssignmentService : BaseService, IAssignmentService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IAssignmentListRepository _assignmentListRepository;

    public AssignmentService(
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

    public async Task<PagedViewModel<AssignmentViewModel>> Search(AssignmentSearchInputModel inputModel)
    {
        var filter = Mapper.Map<AssignmentFilter>(inputModel);
        var result = await _assignmentRepository.Search(GetUserId(), filter, inputModel.PerPage, inputModel.Page);

        return new PagedViewModel<AssignmentViewModel>
        {
            Items = Mapper.Map<List<AssignmentViewModel>>(result.Items),
            Total = result.Total,
            Page = result.Page,
            PerPage = result.PerPage,
            PageCount = result.PageCount
        };
    }

    public async Task<AssignmentViewModel?> GetById(string id)
    {
        if (!GuidUtils.isValidGuid(id))
        {
            Notificator.Handle("O id informado é inválido");
            return null;
        }

        var getAssignment = await _assignmentRepository.GetById(id, GetUserId());

        if (getAssignment != null) return Mapper.Map<AssignmentViewModel>(getAssignment);
        
        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<AssignmentViewModel?> Create(AddAssignmentInputModel inputModel)
    {
        if (!GuidUtils.isValidGuid(inputModel.AssignmentListId))
        {
            Notificator.Handle("O id informado é inválido");
            return null;
        }

        if (!inputModel.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
            return null;
        }

        var getAssignment = await _assignmentListRepository.GetById(Guid.Parse(inputModel.AssignmentListId));

        if (getAssignment == null)
        {
            Notificator.Handle("Não foi possível encontrar a lista de tarefas correspondente.");
            return null;
        }

        var assignment = Mapper.Map<Assignment>(inputModel);
        assignment.UserId = Guid.Parse(GetUserId());

        await _assignmentRepository.Create(assignment);

        return Mapper.Map<AssignmentViewModel>(assignment);
    }

    public async Task<AssignmentViewModel?> Update(string id, UpdateAssignmentInputModel inputModel)
    {
        bool isIdInvalid = !Guid.TryParse(id, out _) || !Guid.TryParse(inputModel.Id, out _);
        
        if (id != inputModel.Id || isIdInvalid)
        {
            Notificator.Handle("O id informado é inválido");
            return null;
        }
        
        if (!inputModel.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
            return null;
        }

        var getAssignment = await _assignmentRepository.GetById(id, GetUserId());

        if (getAssignment == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        var result = Mapper.Map(inputModel, getAssignment);

        await _assignmentRepository.Update(getAssignment);

        return Mapper.Map<AssignmentViewModel>(result);
    }

    public async Task Delete(string id)
    {
        if (!GuidUtils.isValidGuid(id))
        {
            Notificator.Handle("O id informado é inválido");
            return;
        }

        var getAssignment = await _assignmentRepository.GetById(id, GetUserId());

        if (getAssignment == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        await _assignmentRepository.Delete(getAssignment);
    }

    public async Task MarkConcluded(string id)
    {
        if (!GuidUtils.isValidGuid(id))
        {
            Notificator.Handle("O id informado é inválido");
            return;
        }

        var assignment = await _assignmentRepository.GetById(id, GetUserId());

        if (assignment == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        assignment.SetConcluded();

        await _assignmentRepository.Update(assignment);
    }

    public async Task MarkDesconcluded(string id)
    {
        if (!GuidUtils.isValidGuid(id))
        {
            Notificator.Handle("O id informado é inválido");
            return;
        }

        var assignment = await _assignmentRepository.GetById(id, GetUserId());

        if (assignment == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        assignment.SetUnconcluded();

        await _assignmentRepository.Update(assignment);
    }

    private string GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userId ?? string.Empty;
    }
}