using Domain.Dtos;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Projects.ProjectSettings.Components;

public partial class ProjectMembersManager
{
    [Inject]
    public required IProjectMemberService ProjectMemberService { get; init; }

    [Inject]
    public required IRoleService RoleService { get; init; }

    [CascadingParameter]
    public required ApplicationState ApplicationState { get; init; }

    [Parameter, EditorRequired]
    public Guid ProjectId { get; set; }

    public IEnumerable<RoleDto> Roles { get; set; } = [];

    public IEnumerable<ProjectMemberDto> Members { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        var projectMembersResult = await ProjectMemberService
            .GetProjectMembersAsync(ProjectId);

        if (projectMembersResult.IsFailure)
        {
            ApplicationState.ErrorMessage = projectMembersResult.Error!.Message;
            return;
        }

        Members = projectMembersResult.Value;

        var rolesResult = await RoleService
            .GetRolesAsync();

        if (rolesResult.IsFailure)
        {
            ApplicationState.ErrorMessage = rolesResult.Error!.Message;
            return;
        }

        Roles = rolesResult.Value;
    }

    private async Task LoadMembersAsync()
    {
        var result = await ProjectMemberService
            .GetProjectMembersAsync(ProjectId);

        if (result.IsFailure)
        {
            ApplicationState.ErrorMessage = result.Error!.Message;
            return;
        }

        Members = result.Value;
    }
}
