using Client.Intertop;
using Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Client.Components.Modules.VideoChat.Components;

public partial class CameraSelector
{
    [Inject] private IJSRuntime JavaScript { get; set; } = default!;

    private async Task<Device[]> GetVideoDevices()
        => await JavaScript.GetVideoDevicesAsync();

    private async Task StartVideo(string deviceId)
        => await JavaScript.StartVideoAsync(deviceId, "#camera");

    private Task OnCameraChanged(string deviceId)
    {
        Console.WriteLine($"Camera switched to: {deviceId}");
        return Task.CompletedTask;
    }
}
