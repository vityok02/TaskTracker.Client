using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Projects.Components;

public sealed partial class ProjectForm
{
    [Inject]
    public required IProjectService ProjectService { get; set; }

    [Parameter]
    public bool Visible { get; set; }

    [Parameter]
    public EventCallback OnProjectSaved { get; set; }

    [CascadingParameter]
    public required ApplicationState ApplicationState { get; set; }

    [Parameter]
    public bool IsEdit { get; set; }

    [Parameter]
    public Guid ProjectId { get; set; }

    [Parameter]
    public ProjectModel ProjectModel { get; set; } = new ProjectModel();

    [Parameter]
    public EventCallback<bool> VisibleChanged { get; set; }

    private string Title => IsEdit ? "Edit project" : "Create project";

    private string ButtonText => IsEdit ? "Save" : "Create";

    private async Task Submit()
    {
        var result = IsEdit
            ? await ProjectService
                .UpdateProjectAsync(ProjectModel)
            : await ProjectService
                .CreateProjectAsync(ProjectModel);

        if (result.IsFailure)
        {
            ApplicationState.ErrorMessage = result.Error!.Message;
            return;
        }

        await OnProjectSaved.InvokeAsync();

        Visible = false;
    }

    private void Cancel()
    {
        VisibleChanged.InvokeAsync(false);
    }
}