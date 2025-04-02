using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Client.Components.Layout;

public sealed partial class Header
{
    [Inject]
    public required ILocalStorageService Storage { get; init; }

    [CascadingParameter]
    public required Task<AuthenticationState> AuthenticationStateTask { get; set; }

    [CascadingParameter]
    public ApplicationState? AppState { get; set; }

    private string UserName { get; set; } = string.Empty;

    private string AvatarUrl { get; set; } = string.Empty;

    private string UserId { get; set; } = string.Empty;

    protected override async Task OnAfterRenderAsync(bool isFirstRender)
    {
        if (!isFirstRender)
        {
            return;
        }

        var authState = await AuthenticationStateTask;
        var user = authState.User;

        if (user.Identity is { IsAuthenticated: true })
        {
            UserName = user.Identity.Name ?? "Unknown User";
            
            if (isFirstRender)
            {
                AvatarUrl = await Storage
                    .GetItemAsStringAsync("AvatarUrl")
                    ?? user.FindFirstValue(ClaimTypes.Uri)
                    ?? string.Empty;
            }

            UserId = user.FindFirst(c
                => c.Type == JwtRegisteredClaimNames.Sub)?
                .Value ?? string.Empty;
        }

        StateHasChanged();
    }
}
