using AntDesign;
using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Projects;

public sealed partial class ProjectSettings
{
    [Inject]
    public required IProjectService ProjectService { get; init; }

    [Inject]
    public required IProjectMemberService ProjectMemberService { get; init; }

    [Inject]
    public required IRoleService RoleService { get; init; }

    [Inject]
    public required NotificationService Notice { get; init; }

    [CascadingParameter]
    public required ApplicationState ApplicationState { get; set; }

    [Parameter]
    public Guid ProjectId { get; set; }

    [Parameter]
    public ProjectModel ProjectModel { get; set; } = new ProjectModel();

    public ProjectDto Project { get; set; } = new();

    public IEnumerable<ProjectMemberDto> Members { get; set; } = [];

    public IEnumerable<RoleDto> Roles { get; set; } = [];

    protected override async Task OnParametersSetAsync()
    {
        if (ProjectId == Guid.Empty)
        {
            return;
        }

        await HandleRequest(() => ProjectService
            .GetProjectAsync(ProjectId), value => Project = value);

        ProjectModel = new ProjectModel
        {
            Id = Project.Id,
            Name = Project.Name,
            Description = Project.Description,
        };

        await HandleRequest(() => ProjectMemberService
            .GetProjectMembersAsync(ProjectId), value => Members = value);

        await HandleRequest(RoleService
            .GetRolesAsync, value => Roles = value);
    }

    private async Task LoadMembersAsync()
    {
        var result = await ProjectMemberService
            .GetProjectMembersAsync(ProjectId);

        Members = result.Value;

        await HandleRequest(() =>
            ProjectMemberService.GetProjectMembersAsync(ProjectId),
            value => Members = value);
    }

    private async Task UpdateProject()
    {
        var result = await ProjectService
            .UpdateProjectAsync(ProjectModel);

        if (result.IsFailure)
        {
            ApplicationState.ErrorMessage = result.Error!.Message;
            return;
        }

        await Notice.Success(new NotificationConfig()
        {
            Message = "Project updated successfully"
        });
    }
}
