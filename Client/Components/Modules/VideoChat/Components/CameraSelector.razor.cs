using Client.Extensions;
using Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Client.Components.Modules.VideoChat.Components;

public partial class CameraSelector
{
    [Inject]
    public required IJSRuntime JavaScript { get; set; }

    private async Task<Device[]> GetVideoDevices()
    {
        return await JavaScript.GetVideoDevicesAsync();
    }

    private async Task StartVideo(string deviceId)
    {
        await JavaScript.StartVideoAsync(deviceId, "#camera");
    }

    private async Task ToggleDevice(string deviceId)
    {
        await JavaScript.ToggleCameraAsync(deviceId);
    }
}
