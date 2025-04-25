using Client.Intertop;
using Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Client.Components.Modules.VideoChat.Components;

public partial class Microphones
{
    [Inject]
    public required IJSRuntime JavaScript { get; set; }

    [Parameter]
    public EventCallback<string> MicrophoneChanged { get; set; }

    protected Device[]? Devices { get; private set; }
    protected bool IsLoading => Devices == null;
    private string? _activeMic;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Devices = await JavaScript.GetAudioDevicesAsync();
            StateHasChanged();
        }
    }

    protected async ValueTask SelectMicrophone(string deviceId)
    {
        await JavaScript.SetMicrophoneAsync(deviceId);
        _activeMic = deviceId;

        if (MicrophoneChanged.HasDelegate)
        {
            await MicrophoneChanged.InvokeAsync(_activeMic);
        }
    }
}
