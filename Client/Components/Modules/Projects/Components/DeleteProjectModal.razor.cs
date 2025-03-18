using Domain.Models;
using Microsoft.AspNetCore.Components;

namespace Client.Components.Modules.Projects.Components;

public partial class DeleteProjectModal
{
    [Parameter, EditorRequired]
    public string ProjectName { get; set; } = string.Empty;

    [Parameter, EditorRequired]
    public bool Visible { get; set; }

    [Parameter]
    public EventCallback<string> ProjectNameChanged { get; set; }

    [Parameter]
    public EventCallback OnConfirm { get; set; }

    [Parameter]
    public EventCallback<bool> VisibleChanged { get; set; }

    private ConfirmModel ConfirmModel { get; set; } = new();

    private async Task Confirm()
    {
        if (CanDelete())
        {
            await OnConfirm.InvokeAsync();
            await Close();
        }
    }

    private async Task Close()
    {
        await ProjectNameChanged.InvokeAsync(string.Empty);
        await VisibleChanged.InvokeAsync(false);
    }

    private bool CanDelete()
    {
        return ProjectName == ConfirmModel.Input;
    }

    private void UpdateState(ChangeEventArgs e)
    {
        ConfirmModel.Input = e.Value?.ToString() ?? string.Empty;
        StateHasChanged();
    }
}
