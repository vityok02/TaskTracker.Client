using AntDesign;
using Domain.Dtos;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Tasks;

public partial class TasksCalendar
{
    [Inject]
    public required ITaskService TaskService { get; init; }

    [Inject]
    public required IStateService StateService { get; init; }
    [CascadingParameter]

    public required ApplicationState AppState { get; init; }

    [Parameter]
    public required Guid ProjectId { get; init; }

    private List<TaskDto> Tasks { get; set; } = [];

    private IEnumerable<StateDto> States { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        var tasksTask = TaskService.GetAllAsync(ProjectId);
        var statesTask = StateService.GetAllAsync(ProjectId);

        await Task.WhenAll(tasksTask, statesTask);

        var tasksResponse = await tasksTask;
        var stateResponse = await statesTask;

        if (tasksResponse.IsFailure)
        {
            AppState.ErrorMessage = tasksResponse.Error!.Message;
            return;
        }

        Tasks = tasksResponse.Value
            .ToList();

        if (stateResponse.IsFailure)
        {
            AppState.ErrorMessage = stateResponse.Error!.Message;
            return;
        }

        States = stateResponse.Value;
    }

    private List<TaskDto> GetListData(DateTime date)
    {
        return Tasks
    .Where(t =>
        t.StartDate.HasValue &&
        (
            // Якщо є EndDate — беремо по діапазону
            (t.EndDate.HasValue && t.StartDate.Value.Date <= date.Date && t.EndDate.Value.Date >= date.Date)
            ||
            // Якщо EndDate немає — тільки в день StartDate
            (!t.EndDate.HasValue && t.StartDate.Value.Date == date.Date)
        ))
    .ToList();
    }
}
