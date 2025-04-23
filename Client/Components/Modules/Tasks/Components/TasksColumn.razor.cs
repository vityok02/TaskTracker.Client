using AntDesign;
using Client.Extensions;
using Client.Services;
using Domain.Constants;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;
using Result = Domain.Abstract.Result;

namespace Client.Components.Modules.Tasks.Components;

public partial class TasksColumn
{
    [Inject]
    public required ITaskService TaskService { get; init; }

    [Inject]
    public required IStateService StateService { get; init; }

    [Inject]
    public required ModalService ModalService { get; init; }

    [Inject]
    public required DeleteStateConfirmationService DeleteConfirmationService { get; set; }

    [CascadingParameter]
    public required ApplicationState AppState { get; init; }

    [Parameter]
    public Guid ProjectId { get; set; } = new();

    [Parameter]
    public TaskModel TaskModel { get; set; } = new();

    [Parameter]
    public EventCallback OnChange { get; set; }

    [Parameter]
    public EventCallback<Guid> OnShowInput { get; set; }

    [Parameter]
    public EventCallback<Guid> OnOpenDetails { get; set; }

    [Parameter]
    public List<TaskDto> Tasks { get; set; } = [];

    [Parameter]
    public EventCallback<List<TaskDto>> TasksChanged { get; set; }

    [Parameter]
    public StateDto State { get; set; } = new();

    [Parameter]
    public EventCallback<(bool isEdit, Guid id)> OnOpenUpdateStateForm { get; set; }

    [Parameter]
    public string? Role { get; set; }

    protected override void OnInitialized()
    {
    }

    public async Task OnDrop(TaskDto task, List<TaskDto> tasks)
    {
        int index = tasks.IndexOf(task);

        var belowItem = index < tasks.Count - 1
            ? tasks[index + 1]
            : null;

        Result result;

        if (task.StateId != State.Id)
        {
            result = await UpdateStateAsync(task, belowItem?.Id);
        }
        else
        {
            result = await ReorderTasksAsync(task, belowItem?.Id);
        }

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
        }
    }

    private async Task<Result> UpdateStateAsync(TaskDto task, Guid? beforeTaskId)
    {
        task.StateId = State.Id;

        var updateStateModel = new UpdateTaskStateModel
        {
            StateId = task.StateId,
            BeforeTaskId = beforeTaskId
        };

        return await TaskService
            .UpdateStateAsync(ProjectId, task.Id, updateStateModel);
    }

    private async Task<Result> ReorderTasksAsync(TaskDto task, Guid? beforeTaskId)
    {
        var reorderModel = new ReorderTasksModel
        {
            BeforeTaskId = beforeTaskId,
        };

        return await TaskService
            .ReorderAsync(ProjectId, task.Id, reorderModel);
    }

    private void OpenUpdateStateForm()
    {
        OnOpenUpdateStateForm.InvokeAsync((true, State.Id));
    }

    private async Task ShowInputAsync(Guid stateId)
    {
        await OnShowInput.InvokeAsync(stateId);
    }

    private async Task OpenDetailsAsync(Guid taskId)
    {
         await OnOpenDetails.InvokeAsync(taskId);
    }

    private async Task DeleteTask(Guid taskId)
    {
        var result = await TaskService
            .DeleteAsync(ProjectId, taskId);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        await OnChange.InvokeAsync();
    }

    private async Task DeleteState(Guid stateId)
    {
        var result = await StateService
            .DeleteAsync(ProjectId, stateId);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        await OnChange.InvokeAsync();
    }

    private async Task ShowTaskDeleteConfirmAsync(Guid taskId)
    {
        await ModalService.ConfirmAsync(new ConfirmOptions()
        {
            Title = "Delete task?",
            Content = "Are you sure you want to delete this item from this project?",
            Icon = RenderFragments.DeleteIcon,
            OnOk = (e) => DeleteTask(taskId),
            OnCancel = (e) => Task.CompletedTask,
            OkText = "Delete",
            CancelText = "Cancel",
            OkButtonProps = new ButtonProps
            {
                Danger = true
            }
        });
    }

    private bool AllowsDrag()
    {
        return Role
            is Roles.Admin
            or Roles.Contributor;
    }

    private async Task ShowStateDeleteConfirmAsync(Guid stateId)
    {
        await DeleteConfirmationService
            .ShowStateDeleteConfirmAsync(stateId, DeleteState);
    }
}
