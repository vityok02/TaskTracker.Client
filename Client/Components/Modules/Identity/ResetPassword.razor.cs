using Domain.Models.Identity;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.ApiServices;

namespace Client.Components.Modules.Identity;

public partial class ResetPassword
{
    [Inject]
    public required IIdentityService IdentityService { get; init; }

    [Inject]
    public required NavigationManager NavManager { get; init; }

    [Parameter]
    public required string ResetToken { get; set; }

    private SetPasswordModel SetPasswordModel { get; set; } = new();

    public async Task Submit()
    {
        SetPasswordModel.ResetToken = ResetToken;

        var result = await IdentityService
            .SetPasswordAndAuthorize(SetPasswordModel);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        NavManager.NavigateTo("/");
    }
}
