using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Services.Services.Components;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Client.Components.Modules.User;

[Authorize]
public sealed partial class Details
{
    [Inject]
    public required IUserService UserService { get; init; }

    [Inject]
    public required AuthenticationStateProvider AuthStateProvider { get; init; }

    [Parameter]
    public Guid UserId { get; set; }

    private bool IsAuthorized { get; set; }

    private UserDto? User { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        var result = await UserService
            .GetUserAsync(UserId);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;

            NavManager.NavigateTo("/");
        }

        IsAuthorized = await IsUserAuthorizedAsync();

        User = result.Value;
    }

    private async Task<bool> IsUserAuthorizedAsync()
    {
        var authState = await AuthStateProvider
                    .GetAuthenticationStateAsync();

        var user = authState.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            var subClaim = user
                .FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (Guid.TryParse(subClaim, out Guid tokenUserId))
            {
                return (UserId == tokenUserId);
            }
        }

        return false;
    }
}