using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Tasks.Components;

public partial class TaskColumn
{
    [Inject]
    public required ITaskService TaskService { get; init; }

    [Inject]
    public required IStateService StateService { get; init; }

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
    public IEnumerable<TaskDto> Tasks { get; set; } = [];

    [Parameter]
    public StateDto State { get; set; } = new();

    [Parameter]
    public EventCallback<(bool isEdit, Guid id)> OnOpenUpdateStateForm { get; set; }

    public bool StateFormVisible { get; set; } = false;

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
            .DeleteTaskAsync(ProjectId, taskId);

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
}
