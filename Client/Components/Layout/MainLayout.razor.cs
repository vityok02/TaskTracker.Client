using AntDesign;
using Microsoft.AspNetCore.Components;

namespace Client.Components.Layout;

public partial class MainLayout : IDisposable
{
    [Inject]
    public required NotificationService Notice { get; set; }

    [CascadingParameter]
    public ApplicationState? AppState { get; set; }

    protected override void OnInitialized()
    {
        if (AppState is not null)
        {
            AppState.OnNotification += OnNotificationHandler;
        }
    }

    public void Dispose()
    {
        if (AppState is not null)
        {
            AppState.OnNotification -= OnNotificationHandler;
        }
    }

    private void OnNotificationHandler()
    {
        InvokeAsync(ShowNotification);
    }

    private async Task ShowNotification()
    {
        if (AppState is not null)
        {
            await Notice.Error(new NotificationConfig()
            {
                Message = AppState.ErrorMessage,
            });
        }
    }
}
