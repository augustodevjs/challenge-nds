using AutoMapper;
using Todo.Domain.Filter;
using Todo.Domain.Models;
using Todo.Core.Extensions;
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
        IAssignmentListRepository assignmentListRepository
    ) : base(mapper, notificator)
    {
        _httpContextAccessor = httpContextAccessor;
        _assignmentRepository = assignmentRepository;
        _assignmentListRepository = assignmentListRepository;
    }

    public async Task<PagedViewModel<AssignmentViewModel>> Search(AssignmentSearchInputModel inputModel)
    {
        var filter = Mapper.Map<AssignmentFilter>(inputModel);
        var result = await _assignmentRepository.Search(_httpContextAccessor.GetUserId(), filter, inputModel.PerPage,
            inputModel.Page);

        return new PagedViewModel<AssignmentViewModel>
        {
            Items = Mapper.Map<List<AssignmentViewModel>>(result.Items),
            Total = result.Total,
            Page = result.Page,
            PerPage = result.PerPage,
            PageCount = result.PageCount
        };
    }

    public async Task<AssignmentViewModel?> GetById(int id)
    {
        var getAssignment =
            await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());

        if (getAssignment != null) return Mapper.Map<AssignmentViewModel>(getAssignment);

        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<AssignmentViewModel?> Create(AddAssignmentInputModel inputModel)
    {
        var assignment = Mapper.Map<Assignment>(inputModel);
        assignment.UserId = _httpContextAccessor.GetUserId() ?? 0;

        if (!assignment.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
            return null;
        }

        var existingAssignmentList =
            await _assignmentListRepository.FirstOrDefault(u => u.Id == assignment.AssignmentListId);

        if (existingAssignmentList == null)
        {
            Notificator.Handle("Não existe essa lista de tarefa");
            return null;
        }

        var existingAssignment =
            await _assignmentRepository.FirstOrDefault(u => u.Description == assignment.Description);

        if (existingAssignment != null)
        {
            Notificator.Handle("Já existe uma tarefa cadastrada com essas informações");
        }

        _assignmentRepository.Create(assignment);

        if (await _assignmentRepository.UnityOfWork.Commit())
            return Mapper.Map<AssignmentViewModel>(assignment);

        Notificator.Handle("Não foi possível cadastrar a tarefa");
        return null;
    }


    public async Task<AssignmentViewModel?> Update(int id, UpdateAssignmentInputModel inputModel)
    {
        if (id != inputModel.Id)
        {
            Notificator.Handle("Os ids não conferem");
            return null;
        }

        var getAssignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());

        if (getAssignment == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        var result = Mapper.Map(inputModel, getAssignment);

        if (!result.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
            return null;
        }

        var existingAssignmentList =
            await _assignmentListRepository.FirstOrDefault(u => u.Id == result.AssignmentListId);

        if (existingAssignmentList == null)
        {
            Notificator.Handle("Não existe essa lista de tarefa");
            return null;
        }

        var existingAssignment = await _assignmentRepository.FirstOrDefault(u => u.Description == result.Description);

        if (existingAssignment != null)
        {
            Notificator.Handle("Já existe uma tarefa cadastrada com essas informações");
        }

        _assignmentRepository.Update(getAssignment);

        if (await _assignmentRepository.UnityOfWork.Commit())
            return Mapper.Map<AssignmentViewModel>(result);

        Notificator.Handle("Não foi possível atualizar a tarefa");
        return null;
    }

    public async Task Delete(int id)
    {
        var getAssignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());

        if (getAssignment == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        _assignmentRepository.Delete(getAssignment);

        if (!await _assignmentRepository.UnityOfWork.Commit())
        {
            Notificator.Handle("Não foi possível remover a tarefa");
        }
    }

    public async Task MarkConcluded(int id)
    {
        var assignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());

        if (assignment == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        assignment.SetConcluded();

        _assignmentRepository.Update(assignment);

        if (!await _assignmentRepository.UnityOfWork.Commit())
        {
            Notificator.Handle("Não foi possível marcar a tarefa como concluída");
        }
    }

    public async Task MarkDesconcluded(int id)
    {
        var assignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());

        if (assignment == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        assignment.SetUnconcluded();

        _assignmentRepository.Update(assignment);

        if (!await _assignmentRepository.UnityOfWork.Commit())
        {
            Notificator.Handle("Não foi possível marcar a tarefa como não concluída");
        }
    }

    private async Task<bool> Validate(Assignment assignment)
    {
        if (!assignment.Validar(out var validationResult))
            Notificator.Handle(validationResult.Errors);

        var assignmentExistentId =
            await _assignmentRepository.FirstOrDefault(u => u.AssignmentListId == assignment.AssignmentListId);

        if (assignmentExistentId == null)
        {
            Notificator.Handle("Não existe esse essa lista de tarefa");
        }

        var assignmentExistent =
            await _assignmentRepository.FirstOrDefault(u => u.Description == assignment.Description);

        if (assignmentExistent != null)
            Notificator.Handle("Já existe uma tarefa cadastrada com essas informações");

        return !Notificator.HasNotification;
    }
}