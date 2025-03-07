using AntDesign;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Tasks;

public partial class TaskList
{
    [Inject]
    public required ITaskService TaskService { get; set; }

    [Inject]
    public required IProjectService ProjectService { get; set; }

    [CascadingParameter]
    public required ApplicationState AppState { get; set; }

    [Parameter]
    public Guid ProjectId { get; set; }

    private bool HideInput { get; set; } = true;

    private IEnumerable<TaskDto> Tasks { get; set; } = [];

    private ProjectDto Project { get; set; } = new();

    private TaskModel TaskModel { get; set; } = new();

    private string Title => Project?.Name ?? string.Empty;

    public required Input<string> Input { get; set; }

    private bool _detailsVisible = false;

    private Guid SelectedTaskId { get; set; }

    private bool ProjectDetailsVisible { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task Submit()
    {
        var result = await TaskService
            .CreateTaskAsync(TaskModel, ProjectId);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
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

    private async Task Delete(Guid taskId)
    {
        var result = await TaskService
            .DeleteTaskAsync(ProjectId, taskId);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        await LoadDataAsync();
    }

    private void OpenDetials(Guid taskId)
    {
        SelectedTaskId = taskId;
        _detailsVisible = true;
    }

    private async Task LoadDataAsync()
    {
        var projectResult = await ProjectService
            .GetProjectAsync(ProjectId);

        if (projectResult.IsFailure)
        {
            AppState.ErrorMessage = projectResult.Error!.Message;
            return;
        }

        Project = projectResult.Value;

        var taskResult = await TaskService
            .GetTasksAsync(ProjectId);

        if (taskResult.IsFailure)
        {
            AppState.ErrorMessage = taskResult.Error!.Message;
            return;
        }

        Tasks = taskResult.Value;
    }
}
