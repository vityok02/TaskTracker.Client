using Domain.Dtos;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.ProjectMembers.Components;

public partial class ProjectMemberList
{
    [Inject]
    public required IProjectMemberService ProjectMemberService { get; init; }

    [CascadingParameter]
    public required ApplicationState AppState { get; init; }

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
        var result = await ProjectMemberService
            .DeleteMemberAsync(ProjectId, memberId);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
        }

        await OnMemberChanged.InvokeAsync();
    }
}
