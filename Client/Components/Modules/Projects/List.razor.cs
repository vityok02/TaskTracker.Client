using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Services.Services.Components;

namespace Client.Components.Modules.Projects;

[Authorize]
public sealed partial class List
{
    [Inject]
    public required IProjectService ProjectService { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    private IEnumerable<ProjectDto> Projects { get; set; } = [];

    private string ErrorMessage { get; set; } = string.Empty;
    
    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var result = await ProjectService.GetAllProjectsAsync();

        if (result.IsFailure)
        {
            return;
        }

        Projects = result.Value;
    }
}
