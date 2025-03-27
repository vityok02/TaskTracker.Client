using AntDesign;
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

    [CascadingParameter]
    public required ApplicationState AppState { get; init; }

    [Parameter]
    public Guid ProjectId { get; set; }

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
    public List<TaskDto> StateTasks { get; set; } = [];

    [Parameter]
    public StateDto State { get; set; } = new();

    [Parameter]
    public EventCallback<(bool isEdit, Guid id)> OnOpenUpdateStateForm { get; set; }

    public bool StateFormVisible { get; set; } = false;

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

    private async Task ShowInput(Guid stateId)
    {
        await OnShowInput.InvokeAsync(stateId);
    }

    private async Task OpenDetials(Guid taskId)
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

    private async Task ShowTaskDeleteConfirm(Guid taskId)
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

    private async Task ShowStateDeleteConfirm(Guid stateId)
    {
        await ModalService.ConfirmAsync(new ConfirmOptions()
        {
            Title = "Delete confirmation",
            Content = GetContentFragment(),
            Icon = RenderFragments.DeleteIcon,
            OnOk = (e) => DeleteState(stateId),
            OnCancel = (e) => Task.CompletedTask,
            OkText = "Delete",
            CancelText = "Cancel",
            OkButtonProps = new ButtonProps
            {
                Danger = true
            }
        });

        static RenderFragment GetContentFragment()
        {
            return builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, "This will permanently delete this option from the \"Status\" field.");

                builder.OpenElement(2, "span");
                builder.AddAttribute(3, "class", "fw-bold");
                builder.AddContent(4, " This cannot be undone.");
                builder.CloseElement();

                builder.CloseElement();

                builder.OpenElement(5, "div");
                builder.AddAttribute(6, "class", "custom-description bg-light border border-danger text-danger p-2 rounded mt-2");
                builder.OpenElement(7, "span");
                builder.AddAttribute(8, "class", "text-danger");
                builder.AddContent(9, "Warning:");
                builder.CloseElement();
                builder.AddContent(10, " The option will be permanently deleted from any items in this project.");
                builder.CloseElement();
            };
        }
    }
}
