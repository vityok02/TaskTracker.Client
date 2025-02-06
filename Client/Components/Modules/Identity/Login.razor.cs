using Domain.Models.Identity;
using Microsoft.AspNetCore.Components;
using Services.Interfaces;

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
            return;
        }

        NavigationManager.NavigateTo("/");
    }
}
