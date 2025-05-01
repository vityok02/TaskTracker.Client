using Client.Constants;
using Client.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using Services.ExternalApi;

namespace Client.Components.Modules.VideoChat;

public partial class VideoChat : IAsyncDisposable
{
    [CascadingParameter]
    public required ApplicationState AppState { get; set; }

    [Inject]
    protected IJSRuntime? JavaScript { get; set; }

    [Inject]
    public required ProtectedLocalStorage LocalStorage { get; init; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public required IVideoChatApi VideoChatApi { get; set; }

    [Parameter]
    public Guid ProjectId { get; set; }

    private string? ActiveCamera { get; set; }

    private string? ActiveMicrophone { get; set; }

    private HubConnection _hubConnection = default!;
    private const string baseUrl = "https://localhost:5001";
    private bool _isRendered = false;

    protected override async Task OnInitializedAsync()
    {
        var url = Environment
            .GetEnvironmentVariable(EnvironmentKeys.VideoChatApiUrl)
            ?? $"{baseUrl}{HubEndpoints.NotificationHub}";

        _hubConnection = new HubConnectionBuilder()
            .AddMessagePackProtocol()
            .WithUrl(NavigationManager.ToAbsoluteUri($"{baseUrl}{HubEndpoints.NotificationHub}"))
            .WithAutomaticReconnect()
            .Build();

        await _hubConnection.StartAsync();
    }

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

    protected async Task<bool> TryJoinRoomAsync(string? roomName)
    {
        if (string.IsNullOrWhiteSpace(roomName))
            return false;

        var jwtResult = await VideoChatApi
            .GetTokenAsync(ProjectId);

        var token = jwtResult.Content?.Token;

        if (token is null)
            return false;

        var joined = await JavaScript
            .CreateOrJoinRoomAsync(roomName, token);

        return joined;
    }

    async Task OnLeaveRoomAsync()
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
        await OnLeaveRoomAsync();

        if (_hubConnection is not null && _hubConnection.State != HubConnectionState.Disconnected)
        {
            await _hubConnection.StopAsync();
        }

        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }
    }
}
