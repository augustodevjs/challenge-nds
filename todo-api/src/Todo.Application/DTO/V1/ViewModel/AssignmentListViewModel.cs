namespace Todo.Application.DTO.V1.ViewModel;

public class AssignmentListViewModel : Base.Base
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public List<AssignmentViewModel> Assignments { get; set; } = new();
}