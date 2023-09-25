using AutoMapper;
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

    public async Task<PagedViewModel<AssignmentViewModel>?> SearchAssignments(int id,
        AssignmentSearchInputModel inputModel)
    {
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

    public async Task<AssignmentListViewModel?> GetById(int? id)
    {
        var getAssignmentList = await _assignmentListRepository.GetById(id, GetUserId());

        if (getAssignmentList != null) return Mapper.Map<AssignmentListViewModel>(getAssignmentList);

        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<AssignmentListViewModel?> Create(AddAssignmentListInputModel inputModel)
    {
        var assignmentList = Mapper.Map<AssignmentList>(inputModel);
        assignmentList.UserId = GetUserId();

        if (!await Validate(assignmentList)) return null;

        _assignmentListRepository.Create(assignmentList);
        
        if(await _assignmentListRepository.UnityOfWork.Commit()) 
            return Mapper.Map<AssignmentListViewModel>(assignmentList);
        
        Notificator.Handle("Não foi possível criar a lista de tarefa");
        return null;
    }
    
    public async Task<AssignmentListViewModel?> Update(int id, UpdateAssignmentListInputModel inputModel)
    {
        if (id != inputModel.Id)
        {
            Notificator.Handle("Os ids não conferem");
            return null;
        }
        
        var assignmentList = await _assignmentListRepository.GetById(id, GetUserId());

        if (assignmentList == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }
        
        Mapper.Map(inputModel, assignmentList);
        
        if (!await Validate(assignmentList)) return null;

        _assignmentListRepository.Update(assignmentList);
        
        if(await _assignmentListRepository.UnityOfWork.Commit()) 
            return Mapper.Map<AssignmentListViewModel>(assignmentList);
        
        Notificator.Handle("Não foi possível atualizar a lista de tarefa");
        return null;
    }

    public async Task Delete(int id)
    {
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

        _assignmentListRepository.Delete(assignmentList);

        if (!await _assignmentListRepository.UnityOfWork.Commit())
        {
            Notificator.Handle("Não foi possível remover a lista de tarefa");
        }
    }
    
    private async Task<bool> Validate(AssignmentList assignmentList)
    {
        if(!assignmentList.Validar(out var validationResult))
            Notificator.Handle(validationResult.Errors);
        
        var assignmentExistent = await _assignmentListRepository.FirstOrDefault(u => u.Id == assignmentList.Id);

        if (assignmentExistent != null)
            Notificator.Handle("Já existe uma lista de tarefa cadastrada com essas informaçoes.");

        return !Notificator.HasNotification;
    }

    private int GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Convert.ToInt32(userId);
    }
}