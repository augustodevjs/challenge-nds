using AutoMapper;
using Todo.Application.Contracts;
using Todo.Application.Contracts.Services;
using Todo.Domain.Contracts.Repository;

namespace Todo.Application.Services;

public class AssignmentService : BaseService, IAssignmentService
{
    private readonly IMapper _mapper;
    private readonly IAssignmentRepository _assignmentRepository;
    
    public AssignmentService(
        IMapper mapper, 
        INotificator notificator, 
        IAssignmentRepository assignmentRepository
        ) : base(notificator)
    {
        _mapper = mapper;
        _assignmentRepository = assignmentRepository;
    }
}