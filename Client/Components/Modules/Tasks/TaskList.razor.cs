using AntDesign;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Tasks;

public partial class TaskList
{
    [Inject]
    public required ITaskService TaskService { get; init; }

    [Inject]
    public required IProjectService ProjectService { get; init; }

    [CascadingParameter]
    public required ApplicationState AppState { get; init; }

    [Parameter]
    public Guid ProjectId { get; set; }

    private bool HideInput { get; set; } = true;

    private IEnumerable<TaskDto> Tasks { get; set; } = [];

    private ProjectDto Project { get; set; } = new();

    private TaskModel TaskModel { get; set; } = new();

    private string Title => Project?.Name ?? string.Empty;

    public required Input<string> Input { get; set; }

    private bool _stateFormVisible = false;

    private bool _detailsVisible = false;

    private Guid? _selectedStateId;

    private Guid _selectedTaskId;

    private string? _searchTerm;

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private void OpenUpdateStateForm(Guid stateId)
    {
        _selectedStateId = stateId;
        _stateFormVisible = true;
    }

    private void OpenCreateStateForm()
    {
        _stateFormVisible = true;
    }

    private async Task Submit()
    {
        var result = await TaskService
            .CreateAsync(TaskModel, ProjectId);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        HideInput = true;

        TaskModel = new();

        await LoadDataAsync();
    }

    private async Task ShowInput(Guid stateId)
    {
        TaskModel.StateId = stateId;
        HideInput = false;

        await InvokeAsync(async () =>
        {
            await Task.Delay(50);
            await Input.Focus();
        });
    }

    private void OpenDetails(Guid taskId)
    {
        _selectedTaskId = taskId;
        _detailsVisible = true;
    }

    private async Task SearchTasksByNameAsync(string searchTerm)
    {
        _searchTerm = searchTerm;

        await LoadTasksAsync();
    }

    private async Task ClearSearchAsync()
    {
        _searchTerm = null;

        await LoadTasksAsync();
    }

    private async Task LoadDataAsync()
    {
        var projectResult = await ProjectService
            .GetAsync(ProjectId);

        if (projectResult.IsFailure)
        {
            AppState.ErrorMessage = projectResult.Error!.Message;
            return;
        }

        Project = projectResult.Value;

        Project.States = Project.States
            .OrderBy(x => x.SortOrder)
            .ToList();

        await LoadTasksAsync();
    }

    private async Task LoadTasksAsync()
    {
        var result = await TaskService
            .GetAllAsync(ProjectId, _searchTerm);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        Tasks = result.Value;
    }
}
