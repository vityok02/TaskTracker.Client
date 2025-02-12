using Domain.Abstract;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Components;
using Services.Interfaces.Components;

namespace Client.Components.Modules.Identity;

public sealed partial class Login : ComponentBase
{
    private readonly IIdentityService IdentityService;
    private readonly NavigationManager NavigationManager;

    private LoginModel LoginModel = new();

    private string ErrorMessage { get; set; } = string.Empty;

    public Login(
        IIdentityService identityService,
        NavigationManager navigationManager)
    {
        IdentityService = identityService;
        NavigationManager = navigationManager;
    }


    public async Task Submit()
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
