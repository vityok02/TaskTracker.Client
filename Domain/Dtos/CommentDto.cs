namespace Domain.Dtos;

public class CommentDto
{
    public Guid Id { get; set; }
    public string Comment { get; set; } = string.Empty;

    public UserInfoDto CreatedBy { get; set; } = new();
    public DateTime CreatedAt { get; set; }

    public UserInfoDto? UpdatedBy { get; set; } = new();
    public DateTime? UpdatedAt { get; set; }

    public Guid TaskId { get; set; }
    public string TaskName { get; set; } = string.Empty;
}
