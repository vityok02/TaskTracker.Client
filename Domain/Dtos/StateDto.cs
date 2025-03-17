using Domain.Abstract;

namespace Domain.Dtos;

public class StateDto : AuditableDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public int Number { get; init; }
}
