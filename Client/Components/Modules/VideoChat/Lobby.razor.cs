using Client.Constants;
using Client.Intertop;
using Domain.Dtos.Twilio;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace Client.Components.Modules.VideoChat;

public partial class Lobby
{
    [Inject]
    public required IJSRuntime JavaScript { get; init; }

    [Inject]
    public required ProtectedLocalStorage LocalStorage { get; init; }

    [Inject]
    public required NavigationManager NavigationManager { get; init; }

    [Inject]
    public required IHttpClientFactory HttpClientFactory { get; init; }

    [Inject]
    protected HttpClient Http { get; set; } = null!;

    [Parameter]
    public Guid ProjectId { get; set; }

    private string? _roomName;
    private string? _activeCamera;
    private string? _activeMicrophone;
    private string? _activeRoom;
    private HubConnection? _hubConnection;
    private const string baseUrl = "https://localhost:5001";

    protected override async Task OnInitializedAsync()
    {
        using var httpClient = HttpClientFactory
            .CreateClient();

        //_rooms = await httpClient
        //    .GetFromJsonAsync<List<RoomDetails>>($"{baseUrl}/api/twilio/rooms")
        //    ?? [];

        _hubConnection = new HubConnectionBuilder()
            .AddMessagePackProtocol()
            .WithUrl(NavigationManager
                .ToAbsoluteUri($"{baseUrl}{HubEndpoints.NotificationHub}"))
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<string>(HubEndpoints.RoomsUpdated, OnRoomAdded);

        await _hubConnection.StartAsync();
    }

    async Task OnCameraChanged(string activeCamera)
    {
        await InvokeAsync(() => _activeCamera = activeCamera);
        //await LocalStorage.SetAsync("CameraId", activeCamera);
    }

    async Task OnMicrophoneChanged(string activeMicrophone)
    {
        await InvokeAsync(() => _activeMicrophone = activeMicrophone);
        //await LocalStorage.SetAsync("MicrophoneId", activeMicrophone);
    }

    async Task OnRoomAdded(string roomName) =>
        await Task.CompletedTask;
        //await InvokeAsync(async () =>
        //{
        //    using var httpClient = HttpClientFactory
        //        .CreateClient();

        //    _rooms = await httpClient
        //        .GetFromJsonAsync<List<RoomDetails>>("https://localhost:5001/api/twilio/rooms")
        //        ?? [];

        //    StateHasChanged();
        //});

    protected async ValueTask TryAddRoom(object args)
    {
        if (_roomName is null || _roomName is { Length: 0 })
        {
            return;
        }

        var takeAction = args switch
        {
            KeyboardEventArgs keyboard when keyboard.Key == "Enter" => true,
            MouseEventArgs _ => true,
            _ => false
        };

        if (takeAction)
        {
            var addedOrJoined = await TryJoinRoom(_roomName);
            if (addedOrJoined)
            {
                _roomName = null;
            }
        }
    }

    protected async ValueTask<bool> TryJoinRoom(string? roomName)
    {
        if (roomName is null || roomName is { Length: 0 })
        {
            return false;
        }

        var jwt = await Http
            .GetFromJsonAsync<TwilioJwt>("https://localhost:5001/api/twilio/token");

        if (jwt?.Token is null)
        {
            return false;
        }

        var joined = await JavaScript
            .CreateOrJoinRoomAsync(roomName, jwt.Token);
        if (joined)
        {
            _activeRoom = roomName;
            await _hubConnection
                .InvokeAsync(HubEndpoints.RoomsUpdated, _activeRoom);

            NavigationManager
                .NavigateTo($"projects/{ProjectId}/videochat");
        }

        return joined;
    }
}