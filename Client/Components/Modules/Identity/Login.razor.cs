using Client.Extensions;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Identity;

public partial class Login
{
    [Inject]
    public required IIdentityService IdentityService { get; init; }

    [Inject]
    public required AuthenticationStateProvider AuthStateProvider { get; init; }

    private LoginModel LoginModel { get; set; } = new LoginModel();

    private string ErrorMessage { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider
            .GetAuthenticationStateAsync();

        var user = authState.User;

        if (user.Identity?.IsAuthenticated is true)
        {
            NavManager.NavigateTo("/");
        }
    }

    protected async Task Submit()
    {
        var result = await IdentityService
           .LoginAsync(LoginModel);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        NavManager.NavigateToSetTokenPage(result.Value.Token);
    }
}
