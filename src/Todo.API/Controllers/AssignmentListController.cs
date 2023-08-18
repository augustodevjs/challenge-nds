using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Core.Interfaces;

namespace Todo.API.Controllers;

[Authorize]
[Route("AssignmentList")]
public class AssignmentListController : MainController
{
    public AssignmentListController(INotificator notificador) : base(notificador)
    {
    }
}