using Client.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.VideoChat;

public partial class VideoChat : IAsyncDisposable
{
    [CascadingParameter]
    public required ApplicationState AppState { get; init; }

    [Inject]
    public required IJSRuntime JavaScript { get; init; }

    [Inject]
    public required ProtectedLocalStorage LocalStorage { get; init; }

    [Inject]
    public required NavigationManager NavigationManager { get; init; } = null!;

    [Inject]
    public required IVideoChatService VideoChatService { get; init; }

    [Parameter]
    public Guid ProjectId { get; set; }

    private string? ActiveCamera { get; set; }

    private string? ActiveMicrophone { get; set; }

    private bool _isRendered = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _isRendered = true;

            ActiveCamera = (await LocalStorage.GetAsync<string>("CameraId")).Value;
            ActiveMicrophone = (await LocalStorage.GetAsync<string>("MicrophoneId")).Value;

            var joined = await TryJoinRoomAsync(ProjectId.ToString());

            if (!joined)
            {
                NavigationManager.NavigateTo($"/projects/{ProjectId}/lobby");
            }

            StateHasChanged();
        }
    }

    private async Task<bool> TryJoinRoomAsync(string? roomName)
    {
        if (string.IsNullOrWhiteSpace(roomName))
            return false;

        var jwtResult = await VideoChatService
            .GetTokenAsync(ProjectId);

        if (jwtResult.IsFailure)
        {
            AppState.ErrorMessage = jwtResult.Error?.Message
                ?? "Unkrown error occurred!";
            return false;
        }

        var token = jwtResult.Value.Token;

        if (token is null)
            return false;

        var joined = await JavaScript
            .CreateOrJoinRoomAsync(roomName, token);

        return joined;
    }

    private async Task LeaveRoomAsync()
    {
        if (_isRendered)
        {
            try
            {
                await JavaScript.LeaveRoomAsync();
            }
            catch (JSDisconnectedException)
            {
            }
            finally
            {
                NavigationManager.NavigateTo($"/projects/{ProjectId}/lobby", true);
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        await LeaveRoomAsync();
    }
}
