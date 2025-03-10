namespace Domain.Dtos;

public record ProjectMemberDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;

    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;

    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}