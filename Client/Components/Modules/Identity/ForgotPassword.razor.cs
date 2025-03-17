using Domain.Models.Identity;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Identity;

public partial class ForgotPassword
{
    [Inject]
    public required IIdentityService IdentityService { get; init; }

    [CascadingParameter]
    public required ApplicationState AppState { get; init; }

    [Inject]
    public required NavigationManager NavManager { get; init; }

    private ResetPasswordModel ResetPasswordModel { get; set; } = new();

    public async Task Submit()
    {
        var result = await IdentityService
            .ResetPassword(ResetPasswordModel);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        NavManager.NavigateTo("/forgot-password/confirmation");
    }
}
