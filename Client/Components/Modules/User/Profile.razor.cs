using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Services.Interfaces.ApiServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Client.Components.Modules.User;

[Authorize]
public sealed partial class Profile
{
    [Inject]
    public required IUserService UserService { get; init; }

    [Inject]
    public required AuthenticationStateProvider AuthStateProvider { get; init; }

    [CascadingParameter]
    public required ApplicationState AppState { get; init; }

    [Inject]
    public required NavigationManager NavManager { get; init; }

    [Parameter]
    public Guid UserId { get; set; }

    private bool IsAuthorized { get; set; }

    private UserDto? User { get; set; }

    private bool isFirstRender = true;

    protected override async Task OnParametersSetAsync()
    {
        var result = await UserService
            .GetUserByIdAsync(UserId);

        if (result.IsFailure)
        {
            if (isFirstRender)
            {
                AppState.ErrorMessage = result.Error!.Message;
            }

            NavManager.NavigateTo("/");
            return;
        }

        IsAuthorized = await IsUserAuthorizedAsync();

        User = result.Value;

        isFirstRender = false;
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