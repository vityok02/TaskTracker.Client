using Domain.Abstract;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.Components;

namespace Client.Components.Modules.Identity;

public partial class Login
{
    [Inject]
    public required IIdentityService IdentityService { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    private LoginModel LoginModel { get; set; } = new LoginModel();

    private string ErrorMessage { get; set; } = string.Empty;

    protected async Task Submit()
    {
        var result = await IdentityService
           .LoginAsync(LoginModel);

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

        NavigationManager.NavigateTo("/");
    }
}
