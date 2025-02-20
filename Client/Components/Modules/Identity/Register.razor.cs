using Domain.Abstract;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.Components;

namespace Client.Components.Modules.Identity;

public sealed partial class Register : ComponentBase
{
    [Inject]
    public required IIdentityService IdentityService { get; set; }

    [Inject]
    public required NavigationManager NavManager { get; set; }

    [CascadingParameter]
    public required ApplicationState AppState { get; set; }

    private RegisterModel RegisterModel { get; set; } = new RegisterModel();

    private string ErrorMessage { get; set; } = string.Empty;

    public async Task Submit()
    {
        var result = await IdentityService
            .RegisterAsync(RegisterModel);

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;
            return;
        }

        NavManager.NavigateTo("/");
    }
}
