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
    public required ITagService TagService { get; init; }

    [Inject]
    public required IProjectService ProjectService { get; init; }

    [Inject]
    public required INotificationService NotificationService { get; init; }

    [CascadingParameter]
    public required ApplicationState AppState { get; init; }

    [Parameter]
    public Guid ProjectId { get; set; }

    private bool HideInput { get; set; } = true;

    private List<TaskDto> Tasks { get; set; } = [];

    private List<TagDto> Tags { get; set; } = [];

    private ProjectDto? Project { get; set; } = null;

    private TaskModel TaskModel { get; set; } = new();

    private string Title => Project?.Name ?? string.Empty;

    private List<Guid> SelectedTagIds { get; set; } = [];

    public required Input<string> Input { get; set; }

    private bool _stateFormVisible = false;

    private bool _detailsVisible = false;

    private Guid? _selectedStateId;

    private Guid _selectedTaskId;

    private List<TagDto> FilteredTags { get; set; } = [];

    private string SearchTerm { get; set; } = string.Empty;

    private string _searchTagTerm = string.Empty;

    private string SearchTagTerm
    {
        get => _searchTagTerm;
        set
        {
            if (_searchTagTerm == value) return;

            _searchTagTerm = value;

            FilteredTags = Tags
                .Where(t => t.Name.Contains(SearchTagTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();

        if (Project is not null)
        {
            Project.States = Project.States
                .Where(s => s is not null)
                .ToList();
        }

        FilteredTags = Tags;
    }

    public void HandleTagSelectionAsync(TagDto tag)
    {
        if (SelectedTagIds.Contains(tag.Id))
        {
            SelectedTagIds.Remove(tag.Id);
            return;
        }

        SelectedTagIds.Add(tag.Id);
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

        Tasks.Add(result.Value);

        HideInput = true;

        TaskModel = new();

        StateHasChanged();

        await NotificationService
            .Success(new NotificationConfig
            {
                Message = "Task created successfully",
            });
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

    private async Task SearchTasksAsync(string searchTerm)
    {
        SearchTerm = searchTerm;

        await LoadTasksAsync();
    }

    private async Task ClearSearchAsync()
    {
        SearchTerm = null!;

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

        var tagsResult = await TagService
            .GetAllAsync(ProjectId);

        if (tagsResult.IsFailure)
        {
            AppState.ErrorMessage = tagsResult.Error!.Message;
            return;
        }

        Tags = tagsResult.Value
            .OrderBy(x => x.SortOrder)
            .ToList();

        await LoadTasksAsync();
    }

    private void DeleteTaskFromList(Guid taskId)
    {
        Tasks.RemoveAll(x => x.Id == taskId);
    }

    private void DeleteStateFromList(Guid stateId)
    {
        Project!.States
            .RemoveAll(x => x.Id == stateId);
    }

    private void UpdateTask(TaskDto updatedTask)
    {
        var index = Tasks.FindIndex(t => t.Id == updatedTask.Id);
        if (index != -1)
        {
            Tasks[index] = updatedTask;
        }

        StateHasChanged();
    }

    private async Task LoadTasksAsync()
    {
        var result = await TaskService
            .GetAllAsync(ProjectId, SearchTerm, SelectedTagIds.ToArray());

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        Tasks = result.Value.OrderBy(t => t.SortOrder)
            .ToList();
    }
}
