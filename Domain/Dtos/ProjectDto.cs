using Domain.Abstract;

namespace Domain.Dtos;

public class ProjectDto : AuditableDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; } = string.Empty;

    public IEnumerable<StateDto> States { get; init; } = [];
}
