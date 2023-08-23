using System.ComponentModel.DataAnnotations;

namespace Todo.Application.DTO.Assignment;

public class UpdateAssignmentDto : BaseDto
{
    [Required(ErrorMessage = "O campo de descrição não pode ser deixado vazio.")]
    [StringLength(255, MinimumLength = 3,
        ErrorMessage = "O campo de descrição deve conter entre {2} e {1} caracteres.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "O campo de prazo final não pode ser deixado vazio.")]
    public DateTime Deadline { get; set; }

    [Required(ErrorMessage = "O campo AssignmentListId não pode ser deixado vazio.")]
    public string AssignmentListId { get; set; }
}