using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.ProjectMembers.Components;

public partial class ProjectMemberList
{
    [Inject]
    public required IProjectMemberService ProjectMemberService { get; init; }

    [Parameter]
    public Guid ProjectId { get; set; }

    [Parameter]
    public IEnumerable<ProjectMemberDto> Members { get; set; } = [];

    [Parameter]
    public IEnumerable<RoleDto> Roles { get; set; } = [];

    [Parameter]
    public EventCallback OnMemberChanged { get; set; }


    private async Task DeleteMember(Guid memberId)
    {
        await HandleRequest(() => ProjectMemberService
            .DeleteMemberAsync(ProjectId, memberId));

        await OnMemberChanged.InvokeAsync();
    }

    // TODO: fix update
    private async Task UpdateMember(Guid memberId, Guid roleId)
    {
        await HandleRequest(() => ProjectMemberService
            .UpdateMemberAsync(ProjectId, memberId, new ProjectMemberModel
            {
                UserId = memberId,
                RoleId = roleId
            }));
    }
}
