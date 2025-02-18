using AntDesign;
using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Services.ExternalApi;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Client.Components.Modules.User;

[Authorize]
public sealed partial class Details
{
    [Inject]
    public required IUserApi UserApi { get; init; }

    [Inject]
    public required AuthenticationStateProvider AuthStateProvider { get; init; }

    [Parameter]
    public Guid UserId { get; set; }

    private bool IsAuthorized { get; set; }

    private UserDto? User { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var response = await UserApi
            .GetUserAsync(UserId);

        var authState = await AuthStateProvider
            .GetAuthenticationStateAsync();

        var user = authState.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            var subClaim = user
                .FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (Guid.TryParse(subClaim, out Guid tokenUserId))
            {
                IsAuthorized = (UserId == tokenUserId);
            }
        }

        // TODO: Handle response
        User = response.Content;
    }
}