using Domain.Abstract;

namespace Domain.Dtos;

public class CommentDto : AuditableDto
{
    public Guid Id { get; set; }
    public string Comment { get; set; } = string.Empty;

    public Guid TaskId { get; set; }
    public string TaskName { get; set; } = string.Empty;
}
