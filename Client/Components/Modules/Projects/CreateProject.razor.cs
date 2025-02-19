using Client.Extensions;
using Domain.Abstract;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Services.Components;

namespace Client.Components.Modules.Projects;

public partial class CreateProject
{
    [Inject]
    public required IProjectService ProjectService { get; set; }

    [Parameter]
    public bool Visible { get; set; }

    [Parameter]
    public EventCallback OnProjectCreated { get; set; }

    [CascadingParameter]
    public required ApplicationState ApplicationState { get; set; }

    public ProjectModel ProjectModel { get; set; } = new ProjectModel();

    private async Task Submit()
    {
        var result = await ProjectService
            .CreateProjectAsync(ProjectModel);

        if (result.IsFailure)
        {
            ApplicationState.ErrorMessage = result.Error!.Message;
            return;
        }

        await OnProjectCreated.InvokeAsync();

        Visible = false;
    }

    private void Cancel()
    {
        Visible = false;
    }
}