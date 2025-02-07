using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Client.Components.Layout;

public sealed partial class Header : ComponentBase
{
    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    protected string UserName { get; set; } = "Guest";

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;
        var user = authState.User;

        if (user.Identity is { IsAuthenticated: true })
        {
            UserName = user.Identity.Name ?? "Unknown User";
        }
    }
}
