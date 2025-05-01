using Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Client.Components.Modules.VideoChat.Components;

public partial class DeviceSelector
{
    [Inject]
    public required ProtectedLocalStorage LocalStorage { get; set; } = default!;

    [Parameter]
    public required DeviceType DeviceType { get; set; }

    [Parameter]
    public required Func<Task<Device[]>> GetDevices { get; set; }

    [Parameter]
    public required Func<string, Task> SetDevice { get; set; }

    [Parameter]
    public required string StorageKey { get; set; }

    [Parameter]
    public EventCallback<string?> OnToggleDevice { get; set; }

    private bool IsDeviceEnabled { get; set; } = true;

    private Device[]? Devices { get; set; }

    private string? ActiveDevice { get; set; }

    private bool IsLoading { get; set; } = true;

    private bool HasDevices => Devices != null && Devices.Length > 0;

    private bool IsDropdownOpen { get; set; } = false;

    private void ToggleDropdown()
    {
        IsDropdownOpen = !IsDropdownOpen;
    }

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
        var result = await LocalStorage
            .GetAsync<string>(StorageKey);

        return result.Success
            ? result.Value
            : null;
    }

    private async Task SelectDevice(string deviceId)
    {
        ActiveDevice = deviceId;

        await SetDevice(deviceId);
        await LocalStorage
            .SetAsync(StorageKey, deviceId);

        StateHasChanged();
    }

    private async Task ToggleDevice()
    {
        var task = OnToggleDevice.HasDelegate
            ? OnToggleDevice.InvokeAsync(ActiveDevice)
            : OnToggleDevice.InvokeAsync();

        await task;

        IsDeviceEnabled = !IsDeviceEnabled;
    }
}
