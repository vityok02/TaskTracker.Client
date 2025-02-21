using Client.Extensions;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.Components;

namespace Client.Components.Modules.Identity;

public sealed partial class Register
{
    [Inject]
    public required IIdentityService IdentityService { get; set; }

    private RegisterModel RegisterModel { get; set; } = new RegisterModel();

    public async Task Submit()
    {
        var result = await IdentityService
            .RegisterAsync(RegisterModel);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        NavManager.NavigateToSetTokenPage(result.Value.Token.Token);
    }
}
