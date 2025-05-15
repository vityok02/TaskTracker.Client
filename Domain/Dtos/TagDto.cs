using Domain.Abstract;

namespace Domain.Dtos;

public class TagDto : AuditableDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Color { get; set; } = null!;

    public int SortOrder { get; set; }

    public Guid ProjectId { get; set; }
}
