using Client.Intertop;
using Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Client.Components.Modules.VideoChat.Components;

public partial class MicrophoneSelector
{
    [Inject] private IJSRuntime JavaScript { get; set; } = default!;

    private async Task<Device[]> GetAudioDevices()
        => await JavaScript.GetAudioDevicesAsync();

    private async Task StartAudio(string deviceId)
        => await JavaScript.SetMicrophoneAsync(deviceId);

    private Task OnMicrophoneChanged(string deviceId)
    {
        Console.WriteLine($"Microphone switched to: {deviceId}");
        return Task.CompletedTask;
    }
}
