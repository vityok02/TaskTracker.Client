using Domain.Dtos;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Projects.ProjectSettings;

public sealed partial class ProjectSettings
{
    [Inject]
    public required IProjectService ProjectService { get; init; }

    [CascadingParameter]
    public required ApplicationState ApplicationState { get; set; }

    [Parameter]
    public Guid ProjectId { get; set; }

    public ProjectDto Project { get; set; } = new();

    public string? Role { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        if (ProjectId == Guid.Empty)
        {
            return;
        }

        var projectResult = await ProjectService
            .GetAsync(ProjectId);

        if (projectResult.IsFailure)
        {
            ApplicationState.ErrorMessage = projectResult.Error!.Message;
            return;
        }

        Project = projectResult.Value;

        Role = Project.Role.Name;
    }

    private void UpdateProject(ProjectDto projectDto)
    {
        Project = projectDto;
    }
}
