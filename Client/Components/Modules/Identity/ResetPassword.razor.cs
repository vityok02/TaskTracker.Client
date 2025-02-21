using Domain.Models.Identity;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.Components;

namespace Client.Components.Modules.Identity;

public partial class ResetPassword
{
    [Inject]
    public required IIdentityService IdentityService { get; init; }

    public required string ResetToken { get; set; }

    private SetPasswordModel SetPassword { get; set; } = new();

    public async Task Submit()
    {
        SetPassword.ResetToken = ResetToken;

        var result = await IdentityService
            .SetPasswordAndAuthorize(SetPassword);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        NavManager.NavigateTo("/");
    }
}
