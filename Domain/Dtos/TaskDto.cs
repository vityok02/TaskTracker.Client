﻿using Domain.Abstract;

namespace Domain.Dtos;

public class TaskDto : AuditableDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SortOrder { get; set; }

    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;

    public Guid StateId { get; set; }
    public string StateName { get; set; } = string.Empty;
    public string StateColor { get; init; } = string.Empty;

    public List<TagDto> Tags { get; set; } = [];

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}