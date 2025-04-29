using Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;

namespace Client.Components.Modules.VideoChat.Components;

public partial class DeviceSelector
{
    [Inject]
    public required ProtectedLocalStorage LocalStorage { get; set; } = default!;

    [Inject]
    public required IJSRuntime JS { get; set; }

    [Parameter]
    public required DeviceType DeviceType { get; set; }

    [Parameter]
    public required Func<Task<Device[]>> GetDevices { get; set; }

    [Parameter]
    public required Func<string, Task> SetDevice { get; set; }

    [Parameter]
    public required string StorageKey { get; set; }

    [Parameter]
    public EventCallback<string> DeviceChanged { get; set; }

    private Device[]? Devices;
    private string? ActiveDevice;
    private bool IsLoading = true;

    private bool HasDevices => Devices != null && Devices.Length > 0;

    private string? SelectedDeviceLabel => Devices?.FirstOrDefault(d => d.DeviceId == ActiveDevice)?.Label;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Devices = await GetDevices();
            IsLoading = false;

            ActiveDevice = await LoadSavedDevice();
            if (!string.IsNullOrEmpty(ActiveDevice))
            {
                await SetDevice(ActiveDevice);
            }

            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task<string?> LoadSavedDevice()
    {
        var result = await LocalStorage.GetAsync<string>(StorageKey);
        return result.Success ? result.Value : null;
    }

    private async Task SelectDevice(string deviceId)
    {
        ActiveDevice = deviceId;
        await SetDevice(deviceId);
        await LocalStorage.SetAsync(StorageKey, deviceId);

        if (DeviceChanged.HasDelegate)
            await DeviceChanged.InvokeAsync(deviceId);

        StateHasChanged();
    }
}
