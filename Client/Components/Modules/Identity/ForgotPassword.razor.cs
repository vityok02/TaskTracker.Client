using Domain.Models.Identity;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.Components;

namespace Client.Components.Modules.Identity;

public partial class ForgotPassword
{
    [Inject]
    public required IIdentityService IdentityService { get; init; }

    [Inject]
    public required NavigationManager NavManager { get; init; }

    private ResetPasswordModel ResetPasswordModel { get; set; } = new();

    public async Task Submit()
    {
        var result = await IdentityService
            .ResetPassword(ResetPasswordModel);

        if (result.IsSuccess)
        {
            NavManager.NavigateTo("/forgot-password/confirmation");
        }
    }
}
