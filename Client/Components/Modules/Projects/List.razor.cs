using Domain.Dtos;
using Microsoft.AspNetCore.Components;
using Services.Services.Components;

namespace Client.Components.Modules.Projects;

public sealed partial class List
{
    [Inject]
    public required IProjectService ProjectService { get; set; }

    [CascadingParameter]
    public required ApplicationState AppState { get; set; }

    private IEnumerable<ProjectDto> Projects { get; set; } = [];

    private bool _visible = false;

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var result = await ProjectService.GetAllProjectsAsync();

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        Projects = result.Value;
    }
}
