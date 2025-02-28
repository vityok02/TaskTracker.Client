using Domain.Dtos;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Projects.Components;

public partial class ProjectDetails
{
    [Inject]
    public required IProjectService ProjectService { get; set; }

    [CascadingParameter]
    public required ApplicationState AppState { get; set; }

    [Parameter]
    public Guid ProjectId { get; set; }

    [Parameter]
    public bool Visible { get; set; }

    [Parameter]
    public EventCallback<bool> VisibleChanged { get; set; }

    public ProjectDto Project { get; set; } = new();

    protected override async Task OnParametersSetAsync()
    {
        if (ProjectId == Guid.Empty)
        {
            return;
        }

        var result = await ProjectService
            .GetProjectAsync(ProjectId);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
        }

        Project = result.Value;
    }

    private string GetCreatingDetails()
    {
        return $"Created at {Project.CreatedAt.ToShortDateString()} by {Project.CreatedByName}";
    }

    private string GetUpdatingDetails()
    {
        return $"Updated at {Project.UpdatedAt!.Value.ToShortDateString()} by {Project.UpdatedByName}";
    }

    private void Close()
    {
        VisibleChanged.InvokeAsync(false);
    }
}
