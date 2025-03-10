using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Projects.Components;

public partial class ProjectDetails
{
    [Inject]
    public required IProjectService ProjectService { get; set; }

    [Inject]
    public required IProjectMemberService ProjectMemberService { get; set; }

    [Inject]
    public required IRoleService RoleService { get; set; }

    [CascadingParameter]
    public required ApplicationState AppState { get; set; }

    [Parameter]
    public Guid ProjectId { get; set; }

    [Parameter]
    public bool Visible { get; set; }

    [Parameter]
    public EventCallback<bool> VisibleChanged { get; set; }

    public ProjectDto Project { get; set; } = new();

    public ProjectMemberModel MemberModel { get; set; } = new();

    public IEnumerable<ProjectMemberDto> Members { get; set; } = [];

    public IEnumerable<RoleDto> Roles { get; set; } = [];

    protected override async Task OnParametersSetAsync()
    {
        if (ProjectId == Guid.Empty)
        {
            return;
        }

        await HandleRequest(() => ProjectService.GetProjectAsync(ProjectId), value => Project = value);
        await HandleRequest(() => ProjectMemberService.GetProjectMembersAsync(ProjectId), value => Members = value);
        await HandleRequest(RoleService.GetRolesAsync, value => Roles = value);
    }

    private string GetCreatingDetails()
    {
        return $"Created at {Project.CreatedAt.ToShortDateString()} by {Project.CreatedByName}";
    }

    private string GetUpdatingDetails()
    {
        return $"Updated at {Project.UpdatedAt!.Value.ToShortDateString()} by {Project.UpdatedByName}";
    }

    private async Task LoadMembersAsync()
    {
        var result = await ProjectMemberService.GetProjectMembersAsync(ProjectId);

        Members = result.Value;

        await HandleRequest(() => ProjectMemberService
            .GetProjectMembersAsync(ProjectId));
    }

    private async Task HandleRequest<T>(
        Func<Task<Result<T>>> request,
        Action<T>? onSuccess = null)
    {
        var result = await request();

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        if (onSuccess is not null)
        {
            onSuccess(result.Value);
        }
    }

    private void Close()
    {
        VisibleChanged.InvokeAsync(false);
    }
}
