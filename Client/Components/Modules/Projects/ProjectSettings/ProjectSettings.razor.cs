using AntDesign;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Projects.ProjectSettings;

public sealed partial class ProjectSettings
{
    [Inject]
    public required IProjectService ProjectService { get; init; }

    [Inject]
    public required NotificationService Notice { get; init; }

    [CascadingParameter]
    public required ApplicationState ApplicationState { get; set; }

    [Parameter]
    public Guid ProjectId { get; set; }

    [Parameter]
    public ProjectModel ProjectModel { get; set; } = new ProjectModel();

    public ProjectDto Project { get; set; } = new();

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

        ProjectModel = new ProjectModel
        {
            Name = Project.Name,
            Description = Project.Description,
            StartDate = Project.StartDate,
            EndDate = Project.EndDate,
        };
    }

    private async Task UpdateProject()
    {
        var result = await ProjectService
            .UpdateAsync(ProjectId, ProjectModel);

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
