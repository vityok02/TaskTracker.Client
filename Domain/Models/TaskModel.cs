﻿namespace Domain.Models;

public class TaskModel
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid StateId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
