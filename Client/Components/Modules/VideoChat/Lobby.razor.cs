using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Client.Components.Modules.VideoChat;

public partial class Lobby
{
    [Inject]
    public required NavigationManager NavigationManager { get; init; }

    [Inject]
    public required IJSRuntime JsRuntime { get; init; }

    [Parameter]
    public Guid ProjectId { get; set; }

    private void JoinRoom()
    {
        NavigationManager
            .NavigateTo($"projects/{ProjectId}/videochat");
    }

    private async Task RedirectToProject()
    {
        await JsRuntime.InvokeVoidAsync("videoInterop.disposeCamera");

        NavigationManager
            .NavigateTo($"projects/{ProjectId}/tasks");
    }
}