using Microsoft.AspNetCore.Components;

namespace Client.Components.Modules.VideoChat;

public partial class Lobby
{
    [Inject]
    public required NavigationManager NavigationManager { get; init; }

    [Parameter]
    public Guid ProjectId { get; set; }

    private string? ActiveCamera { get; set; }

    private string? ActiveMicrophone { get; set; }

    private void OnCameraChanged(string activeCamera)
    {
        ActiveCamera = activeCamera;
    }

    private void OnMicrophoneChanged(string activeMicrophone)
    {
        ActiveMicrophone = activeMicrophone;
    }

    private void JoinRoom()
    {
        NavigationManager
            .NavigateTo($"projects/{ProjectId}/videochat");
    }
}