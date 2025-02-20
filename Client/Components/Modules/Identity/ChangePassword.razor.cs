using Domain.Models.Identity;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.Components;

namespace Client.Components.Modules.Identity;

public partial class ChangePassword
{
    [Inject]
    public required IIdentityService IdentityService { get; init; }

    [Inject]
    public required NavigationManager NavManager { get; init; }

    [CascadingParameter]
    public required ApplicationState AppState { get; init; }

    [Parameter]
    public required Guid UserId { get; init; }

    public ChangePasswordModel ChangePasswordModel { get; set; } = new();

    public async Task Submit()
    {
        ChangePasswordModel.UserId = UserId;

        var result = await IdentityService
            .ChangePassword(ChangePasswordModel);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        NavManager.NavigateTo("/");
    }
}
