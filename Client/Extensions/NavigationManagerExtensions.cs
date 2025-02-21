using Microsoft.AspNetCore.Components;

namespace Client.Extensions;

public static class NavigationManagerExtensions
{
    public static void NavigateToSetTokenPage(
        this NavigationManager navigationManager,
        string token)
    {
        navigationManager
            .NavigateTo($"/set-token?token={token}", true);
    }
}
