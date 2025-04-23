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

    private bool IsNameInput { get; set; } = false;

    private bool HasChanges { get; set; } = false;

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
}
