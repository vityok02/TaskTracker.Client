using Client.Helpers;
using Client.Services;
using Domain.Constants;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Projects.ProjectSettings.Components;

public partial class StateList
{
    [Inject]
    public required IStateService StateService { get; init; }

    [Inject]
    public required DeleteStateConfirmationService DeleteStateConfirmationService { get; init; }

    [CascadingParameter]
    public required ApplicationState AppState { get; init; }

    [Parameter, EditorRequired]
    public required Guid ProjectId { get; init; }

    public List<StateDto> States { get; set; } = [];

    private StateModel StateModel { get; set; } = new();

    private bool StateFormVisible { get; set; }

    private Guid? SelectedStateId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var result = await StateService
            .GetAllAsync(ProjectId);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        States = result.Value
            .OrderBy(s => s.SortOrder)
            .ToList();
    }

    private async Task ReorderAsync((int oldIndex, int lastIndex) indices)
    {
        var (oldIndex, newIndex) = indices;

        var items = States;
        var itemToMove = items[oldIndex];

        ReorderListHelper.Reorder(items, oldIndex, newIndex);
        var belowItem = ReorderListHelper.GetBelow(items, newIndex);

        var reorderStateModel = new ReorderStateModel
        {
            BeforeStateId = belowItem?.Id,
        };

        var result = await StateService
            .ReorderAsync(ProjectId, itemToMove.Id, reorderStateModel);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }
    }

    private async Task ShowDeleteConfirmationAsync(Guid stateId)
    {
        await DeleteStateConfirmationService
            .ShowStateDeleteConfirmAsync(stateId, DeleteAsync);
    }

    private async Task DeleteAsync(Guid stateId)
    {
        var result = await StateService
            .DeleteAsync(ProjectId, stateId);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        States.RemoveAll(s => s.Id == stateId);
    }

    private void ShowUpdateForm(Guid stateId)
    {
        StateFormVisible = true;
        SelectedStateId = stateId;
    }

    public void UpdateState(StateDto updatedState)
    {
        var index = States
            .FindIndex(s => s.Id == updatedState.Id);

        States[index] = updatedState;
    }

    private async Task CreateStateAsync()
    {
        var result = await StateService
            .CreateAsync(ProjectId, StateModel);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        StateModel = new();

        States.Add(result.Value);
    }
}
