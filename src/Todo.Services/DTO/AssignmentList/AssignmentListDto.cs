﻿using Todo.Services.DTO.Assignment;

namespace Todo.Services.DTO.AssignmentList;

public class AssignmentListDto : BaseDto
{
    public string Name { get; set; }
    public string Description { get; set; }

    public List<AssignmentDto> Assignment { get; set; }
}