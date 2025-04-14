using AntDesign;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.JsonPatch;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Tasks.Components;

public partial class TaskDetails
{
    [Inject]
    public required ITaskService TaskService { get; init; }

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

    [Inject]
    public required NotificationService Notification { get; init; }

    private TaskModel TaskModel { get; set; } = new();

    private TaskDto? Task { get; set; }

    private bool IsNameInput { get; set; }

    private bool IsDescriptionInput { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        if (ProjectId == Guid.Empty
            || TaskId == Guid.Empty)
        {
            return;
        }

        var result = await TaskService
            .GetAsync(ProjectId, TaskId);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        Task = result.Value;

        InitializeTaskModel();
    }

    private async Task Update()
    {
        var patch = new JsonPatchDocument<TaskModel>();

        if (TaskModel.Name != Task!.Name)
        {
            patch.Replace(t => t.Name, TaskModel.Name);
        }

        if (TaskModel.Description != Task.Description)
        {
            patch.Replace(t => t.Description, TaskModel.Description);
        }

        if (TaskModel.StateId != Task.StateId)
        {
            patch.Replace(t => t.StateId, TaskModel.StateId);
        }

        if (TaskModel.StartDate != Task.StartDate)
        {
            patch.Replace(t => t.StartDate, TaskModel.StartDate);
        }

        if (TaskModel.EndDate != Task.EndDate)
        {
            patch.Replace(t => t.EndDate, TaskModel.EndDate);
        }

        var result = await TaskService
            .PartialUpdateAsync(ProjectId, TaskId, TaskModel);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        Task!.Name = TaskModel.Name;
        Task.Description = TaskModel.Description;

        HideNameInput();
        HideDescriptionInput();

        await Notification.Success(new NotificationConfig()
        {
            Message = "Task successfully updated",
            Placement = NotificationPlacement.Top
        });
    }

    private void ShowNameInput()
    {
        IsNameInput = true;
    }

    private void ShowDescriptionInput()
    {
        if (Task is null)
        {
            return;
        }

        IsDescriptionInput = true;
    }

    private void HideNameInput()
    {
        IsNameInput = false;
        StateHasChanged();
    }

    private void HideDescriptionInput()
    {
        IsDescriptionInput = false;
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
            StateId = Task.StateId
        };
    }

    private void Close()
    {
        VisibleChanged.InvokeAsync(false);
    }
}
