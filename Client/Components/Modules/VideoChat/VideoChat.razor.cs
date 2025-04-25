using Client.Constants;
using Client.Intertop;
using Domain.Dtos.Twilio;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace Client.Components.Modules.VideoChat;

public partial class VideoChat : IAsyncDisposable
{
    [Inject]
    protected IJSRuntime? JavaScript { get; set; }

    [Inject]
    public required ProtectedLocalStorage LocalStorage { get; init; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    protected HttpClient Http { get; set; } = null!;

    [Parameter]
    public Guid ProjectId { get; set; }

    private List<RoomDetails> _rooms = [];

    private string? _roomName;
    private string? _activeCamera;
    private string? _activeRoom;
    private string? _activeMicrophone;
    private HubConnection _hubConnection = default!;
    private const string baseUrl = "https://localhost:5001";

    protected override async Task OnInitializedAsync()
    {
        //_rooms = await Http
        //    .GetFromJsonAsync<List<RoomDetails>>($"{baseUrl}/api/twilio/rooms")
        //    ?? [];

        _hubConnection = new HubConnectionBuilder()
            .AddMessagePackProtocol()
            .WithUrl(NavigationManager.ToAbsoluteUri($"{baseUrl}{HubEndpoints.NotificationHub}"))
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<string>(HubEndpoints.RoomsUpdated, OnRoomAdded);

        await _hubConnection.StartAsync();

        //await TryJoinRoom(ProjectId.ToString());
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await TryJoinRoom(ProjectId.ToString());

            _activeCamera = (await LocalStorage.GetAsync<string>("CameraId")).Value;
            _activeMicrophone = (await LocalStorage.GetAsync<string>("MicrophoneId")).Value;

            StateHasChanged();
        }
    }

    async ValueTask OnLeaveRoom()
    {
        await JavaScript
            .LeaveRoomAsync();

        await _hubConnection
            .InvokeAsync(HubEndpoints.RoomsUpdated, _activeRoom = null);

        if (!string.IsNullOrWhiteSpace(_activeCamera))
        {
            await JavaScript
                .StartVideoAsync(_activeCamera, "#camera");
        }
    }

    async Task OnCameraChanged(string activeCamera) =>
        await InvokeAsync(() => _activeCamera = activeCamera);

    async Task OnMicrophoneChanged(string activeMicrophone) =>
        await InvokeAsync(() => _activeMicrophone = activeMicrophone);

    async Task OnRoomAdded(string roomName) =>
        await Task.CompletedTask;
    //await InvokeAsync(async () =>
    //{
    //    _rooms = await Http
    //        .GetFromJsonAsync<List<RoomDetails>>("https://localhost:5001/api/twilio/rooms") ?? [];
    //    StateHasChanged();
    //});

    //protected async ValueTask TryAddRoom(object args)
    //{
    //    if (_roomName is null || _roomName is { Length: 0 })
    //    {
    //        return;
    //    }

    //    var takeAction = args switch
    //    {
    //        KeyboardEventArgs keyboard when keyboard.Key == "Enter" => true,
    //        MouseEventArgs _ => true,
    //        _ => false
    //    };

    //    if (takeAction)
    //    {
    //        var addedOrJoined = await TryJoinRoom(_roomName);
    //        if (addedOrJoined)
    //        {
    //            _roomName = null;
    //        }
    //    }
    //}

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
        }

        return joined;
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection
            .DisposeAsync();
    }
}