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
            Id = Project.Id,
            Name = Project.Name,
            Description = Project.Description,
        };
    }

    private async Task UpdateProject()
    {
        var result = await ProjectService
            .UpdateAsync(ProjectModel);

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
