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

    public ProjectModel ProjectModel { get; set; } = new ProjectModel();

    private string ErrorMessage { get; set; } = string.Empty;

    private async Task Submit()
    {
        var result = await ProjectService
            .CreateProjectAsync(ProjectModel);

        if (result.IsFailure)
        {
            if (result.Error is ValidationError validationError)
            {
                ErrorMessage = validationError.Errors
                    .FirstOrDefault()?.Message
                        ?? "Unknown validation error";

                return;
            }

            ErrorMessage = result.Error?.Message
                ?? string.Empty;

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