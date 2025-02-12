using Domain.Abstract;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Services.Components;
namespace Client.Components.Modules.Projects;

public sealed partial class CreateComponent : ComponentBase
{
    private readonly IProjectService _projectService;
    private readonly NavigationManager NavigationManager;

    private ProjectModel ProjectModel { get; set; } = new ProjectModel();
    private string ErrorMessage { get; set; } = string.Empty;

    public CreateComponent(
        IProjectService projectService,
        NavigationManager navigationManager)
    {
        _projectService = projectService;
        NavigationManager = navigationManager;
    }

    public async Task Submit()
    {
        var result = await _projectService
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

        NavigationManager
            .NavigateTo("/projects");
    }
}