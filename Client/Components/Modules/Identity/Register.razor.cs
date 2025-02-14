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

    private RegisterModel RegisterModel { get; set; } = new RegisterModel();

    private string ErrorMessage { get; set; } = string.Empty;

    public async Task Submit()
    {
        if (RegisterModel.Password != RegisterModel.ConfirmedPassword)
        {
            ErrorMessage = "Passwords do not match";
            return;
        }

        var result = await IdentityService.RegisterAsync(RegisterModel);

        if (result.IsFailure)
        {
            if (result.Error is ValidationError validationError)
            {
                ErrorMessage = validationError.Errors
                    .FirstOrDefault()?.Message
                        ?? "Unknown validation error";

                return;
            }

            ErrorMessage = result.Error?.Message
                ?? string.Empty;

            return;
        }

        NavManager.NavigateTo("/");
    }
}
