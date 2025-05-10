using AntDesign;
using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.States;

public partial class StateForm
{
    [Inject]
    public required IStateService StateService { get; init; }

    [Inject]
    public required INotificationService NotificationService { get; init; }

    [CascadingParameter]
    public required ApplicationState AppState { get; init; }

    [Parameter]
    public Guid ProjectId { get; set; }

    [Parameter]
    public bool Visible { get; set; }

    [Parameter]
    public EventCallback<bool> VisibleChanged { get; set; }

    [Parameter]
    public Guid? StateId { get; set; }

    [Parameter]
    public EventCallback<Guid?> StateIdChanged { get; set; }

    [Parameter]
    public StateModel? StateModel { get; set; } = new();

    [Parameter]
    public EventCallback<StateDto> OnSubmit { get; set; }

    public bool IsEdit => StateId.HasValue;

    private StateModel _stateModel = new();

    private string Title => IsEdit 
        ? "Edit state" 
        : "Create State";

    private string SubmitButtonText => IsEdit 
        ? "Update"
        : "Create";

    protected override void OnParametersSet()
    {
        if (StateModel is null)
        {
            _stateModel = new StateModel();
            return;
        }

        if (IsEdit)
        {
            _stateModel = new StateModel
            {
                Name = StateModel.Name,
                Number = StateModel.Number,
                Description = StateModel.Description,
                Color = StateModel.Color,
            };
        }
        else
        {
            _stateModel = new();
        }
    }

    private async Task SubmitAsync()
    {
        Result<StateDto> result = IsEdit && StateId is not null
            ? await StateService.UpdateAsync(ProjectId, StateId.Value, _stateModel)
            : await StateService.CreateAsync(ProjectId, _stateModel);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        _stateModel = new();

        await OnSubmit.InvokeAsync(result.Value);

        var task1 = NotificationService.Success(new NotificationConfig()
        {
            Message = $"State {(IsEdit ? "updated" : "created")} successfully"
        });

        var task2 = Close();

        await Task.WhenAll(task1, task2);
    }

    private async Task Close()
    {
        await ResetStateId();
        await VisibleChanged.InvokeAsync(false);
    }

    private async Task ResetStateId()
    {
        await StateIdChanged.InvokeAsync(null);
    }

    private static string GetRadioButtonStyle(string color, string stateColor)
    {
        return $@"
            background: {(color + "50")};
            border: 3px solid {color};
            border-radius: 20px; 
            height: 41px;
            width: 41px";
    }
}
