using Domain.Abstract;

namespace Domain.Dtos;

public class ProjectDto : AuditableDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; } = string.Empty;

    public List<StateDto> States { get; set; } = [];
}
