namespace Domain.Models;

public class TaskModel
{
    public Guid? Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid StateId { get; set; }
}
