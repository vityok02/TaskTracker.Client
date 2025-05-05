using Client.Extensions;
using Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Client.Components.Modules.VideoChat.Components;

public partial class MicrophoneSelector
{
    [Inject]
    public required IJSRuntime JavaScript { get; set; }

    private async Task<Device[]> GetAudioDevices()
        => await JavaScript.GetAudioDevicesAsync();

    private async Task StartAudio(string deviceId)
        => await JavaScript.SetMicrophoneAsync(deviceId);

    private async Task ToggleDevice()
    {
        await JavaScript.ToggleMicrophoneAsync();
    }
}
