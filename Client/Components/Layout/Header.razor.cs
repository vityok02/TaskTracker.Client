using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace Client.Components.Layout;

public sealed partial class Header : ComponentBase
{
    [CascadingParameter]
    public required Task<AuthenticationState> AuthenticationStateTask { get; set; }

    private string UserName { get; set; } = string.Empty;
    private string UserId { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;
        var user = authState.User;

        if (user.Identity is { IsAuthenticated: true })
        {
            UserName = user.Identity.Name ?? "Unknown User";
            UserId = user.FindFirst(c
                => c.Type == JwtRegisteredClaimNames.Sub)?
                .Value ?? string.Empty;
        }
    }
}
