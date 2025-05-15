using AntDesign;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Tasks.Components;

public partial class TaskDetails
{
    [Inject]
    public required ITaskService TaskService { get; init; }

    [Inject]
    public required ITagService TagService { get; init; }

    [Inject]
    public required INotificationService Notification { get; init; }

    [CascadingParameter]
    public required ApplicationState AppState { get; init; }

    [Parameter]
    public Guid TaskId { get; set; }

    [Parameter]
    public Guid ProjectId { get; set; }

    [Parameter]
    public bool Visible { get; set; }

    [Parameter]
    public EventCallback<bool> VisibleChanged { get; set; }

    [Parameter]
    public EventCallback<TaskDto> TaskUpdated { get; set; }

    public List<TagDto> Tags { get; set; } = [];

    private TaskModel TaskModel { get; set; } = new();

    private TaskDto Task { get; set; } = new TaskDto();

    private bool IsNameInput { get; set; } = false;

    private bool HasChanges { get; set; } = false;

    private List<TagDto> FilteredTags { get; set; } = [];

    private string _searchTerm = string.Empty;

    private string SearchTerm
    {
        get => _searchTerm;
        set
        {
            if (_searchTerm == value) return;

            _searchTerm = value;
            SearchTags();
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        if (ProjectId == Guid.Empty
            || TaskId == Guid.Empty)
        {
            return;
        }

        var taskTask = TaskService
            .GetAsync(ProjectId, TaskId);

        var tagsTask = TagService
            .GetAllAsync(ProjectId);

        await System.Threading.Tasks.Task
            .WhenAll(taskTask, tagsTask);

        var taskResult = taskTask.Result;
        var tagsResult = tagsTask.Result;

        if (taskResult.IsFailure)
        {
            AppState.ErrorMessage = taskResult.Error!.Message;
            return;
        }

        if (tagsResult.IsFailure)
        {
            AppState.ErrorMessage = tagsResult.Error!.Message;
            return;
        }

        Task = taskResult.Value;

        Tags = tagsResult.Value
            .ToList();

        FilteredTags = Tags;

        InitializeTaskModel();
    }

    private async Task Update()
    {
        var result = await TaskService
            .UpdateAsync(ProjectId, TaskId, TaskModel);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        Task!.Name = TaskModel.Name;
        Task.Description = TaskModel.Description;

        HideNameInput();

        await TaskUpdated.InvokeAsync(Task);

        await Notification.Success(new NotificationConfig()
        {
            Message = "Task successfully updated!"
        });
    }

    private void ShowNameInput()
    {
        IsNameInput = true;
    }

    private void HideNameInput()
    {
        IsNameInput = false;
        StateHasChanged();
    }

    private void InitializeTaskModel()
    {
        if (Task is null)
        {
            return;
        }

        TaskModel = new TaskModel
        {
            Name = Task.Name,
            Description = Task.Description,
            StartDate = Task.StartDate,
            EndDate = Task.EndDate,
            StateId = Task.StateId
        };
    }

    private void CheckForChanges()
    {
        HasChanges = TaskModel.Description != Task!.Description
                || TaskModel.StartDate != Task.StartDate
                || TaskModel.EndDate != Task.EndDate;
    }

    private void ResetChanges()
    {
        TaskModel.Description = Task!.Description;
        TaskModel.StartDate = Task.StartDate;
        TaskModel.EndDate = Task.EndDate;
        HasChanges = false;
    }

    private void Close()
    {
        VisibleChanged.InvokeAsync(false);
    }

    private async Task HandleTagSelectionAsync(TagDto tag)
    {
        if (Task.Tags.Any(t => t.Id == tag.Id))
        {
            await RemoveTagAsync(tag);
        }
        else
        {
            await AddTagAsync(tag);
        }

        await TaskUpdated.InvokeAsync(Task);

        StateHasChanged();
    }

    private async Task AddTagAsync(TagDto tag)
    {
        var result = await TaskService.AddTagAsync(ProjectId, Task.Id, tag.Id);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        Task.Tags.Add(tag);
    }

    private async Task RemoveTagAsync(TagDto tag)
    {
        var result = await TaskService
            .RemoveTagAsync(ProjectId, Task.Id, tag.Id);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        Task.Tags.RemoveAll(t => t.Id == tag.Id);
    }

    private void SearchTags()
    {
        FilteredTags = Tags
            .Where(t => t.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    private void ClearSearch()
    {
        SearchTerm = string.Empty;
        SearchTags();
    }
}
