using AntDesign;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Projects.ProjectSettings.Components;

public partial class ProjectSettingsForm
{
    [Inject]
    public required IProjectService ProjectService { get; init; }

    [Inject]
    public required INotificationService NotificationService { get; init; }

    [CascadingParameter]
    public required ApplicationState ApplicationState { get; init; }

    [Parameter, EditorRequired]
    public ProjectDto ProjectDto { get; set; } = new();

    public ProjectModel ProjectModel { get; set; } = null!;

    [Parameter]
    public string? Role { get; set; }

    [Parameter]
    public EventCallback<ProjectDto> OnUpdated { get; set; }

    protected override void OnParametersSet()
    {
        ProjectModel = new ProjectModel()
        {
            Name = ProjectDto.Name,
            Description = ProjectDto.Description,
            StartDate = ProjectDto.StartDate,
            EndDate = ProjectDto.EndDate
        };
    }

    private async Task UpdateAsync()
    {
        var result = await ProjectService
            .UpdateAsync(ProjectDto.Id, ProjectModel);

        if (result.IsFailure)
        {
            ApplicationState.ErrorMessage = result.Error!.Message;
            return;
        }

        ProjectDto.Name = ProjectModel.Name;
        ProjectDto.Description = ProjectModel.Description;
        ProjectDto.StartDate = ProjectModel.StartDate;
        ProjectDto.EndDate = ProjectModel.EndDate;

        await OnUpdated.InvokeAsync(ProjectDto);

        await NotificationService.Success(new NotificationConfig()
        {
            Message = "Project updated successfully"
        });
    }
}
