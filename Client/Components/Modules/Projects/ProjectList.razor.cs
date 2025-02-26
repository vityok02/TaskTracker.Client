using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Projects;

public sealed partial class ProjectList
{
    [Inject]
    public required IProjectService ProjectService { get; set; }

    [CascadingParameter]
    public required ApplicationState AppState { get; set; }

    private IEnumerable<ProjectDto> Projects { get; set; } = [];

    private ProjectModel _selectedProjectModel = new();

    private bool _isEdit = false;

    private bool _formVisible = false;

    private bool _detailsVisible = false;

    private Guid _selectedProjectId = Guid.Empty;

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private void ShowDetails(Guid id)
    {
        _selectedProjectId = Guid.Empty;
        StateHasChanged();
        _selectedProjectId = id;
        _detailsVisible = true;
        StateHasChanged();
    }

    private void ShowEditForm(ProjectDto project)
    {
        _selectedProjectModel = new ProjectModel
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description
        };

        _isEdit = true;
        _formVisible = true;
    }

    private void ShowCreateForm()
    {
        _selectedProjectModel = new ProjectModel();
        _isEdit = false;
        _formVisible = true;
    }

    private async Task Delete(Guid id)
    {
        var result = await ProjectService
            .DeleteProjectAsync(id);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var result = await ProjectService
            .GetAllProjectsAsync();

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        Projects = result.Value;
    }
}
