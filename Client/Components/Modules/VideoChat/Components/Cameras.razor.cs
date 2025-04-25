using Client.Intertop;
using Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;

namespace Client.Components.Modules.VideoChat.Components;

public partial class Cameras
{
    [Inject]
    protected IJSRuntime? JavaScript { get; set; }

    [Parameter]
    public EventCallback<string> CameraChanged { get; set; }

    [Inject]
    public required ProtectedLocalStorage LocalStorage { get; set; }

    protected Device[]? Devices { get; private set; }
    protected CameraState State { get; private set; }
    protected bool HasDevices => State == CameraState.FoundCameras;
    protected bool IsLoading => State == CameraState.LoadingCameras;

    private string? SelectedCameraLabel =>
        Devices?.FirstOrDefault(d => d.DeviceId == ActiveCamera)?.Label;

    [Parameter]
    public string? ActiveCamera { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Devices = await JavaScript.GetVideoDevicesAsync();
            State = Devices != null && Devices.Length > 0
                    ? CameraState.FoundCameras
                    : CameraState.Error;

            var cameraToSelect = await LocalStorage.GetAsync<string>("CameraId");

            //var cameraToSelect = Devices?.FirstOrDefault(d => d.DeviceId == ActiveCamera)?.DeviceId
            //                 ?? Devices?.First().DeviceId;

            if (cameraToSelect.Success)
            {
                await SelectCamera(cameraToSelect.Value);
            }

            await InvokeAsync(StateHasChanged);
        }
    }

    protected async ValueTask SelectCamera(string deviceId)
    {
        await JavaScript.StartVideoAsync(deviceId, "#camera");
        ActiveCamera = deviceId;

        if (CameraChanged.HasDelegate)
        {
            await CameraChanged.InvokeAsync(ActiveCamera);
            await LocalStorage.SetAsync("CameraId", ActiveCamera);
        }
    }
}
