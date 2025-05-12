namespace Client.Components.Modules.Identity;

using Microsoft.AspNetCore.Components;

public partial class LoginRedirect
{
    [Inject]
    public required NavigationManager NavManager { get; init; }

    protected override void OnInitialized()
    {
        var currentUri = NavManager.ToBaseRelativePath(NavManager.Uri);

        if (!currentUri.StartsWith("login", StringComparison.OrdinalIgnoreCase) &&
            !currentUri.StartsWith("reset-password", StringComparison.OrdinalIgnoreCase))
        {
            NavManager.NavigateTo("/login", true);
        }
    }
}
