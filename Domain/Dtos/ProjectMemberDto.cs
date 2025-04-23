namespace Domain.Dtos;

public record ProjectMemberDto
{
    public UserInfoDto User { get; set; } = new();

    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;

    public RoleDto Role { get; set; } = default!;
}