using Domain.Models.Identity;
using Microsoft.AspNetCore.Components;
using Services.Interfaces;

namespace Client.Components.Modules.Identity;

public partial class Login
{
    private readonly IIdentityService IdentityService;
    private readonly NavigationManager NavigationManager;

    protected LoginModel LoginModel = new();

    protected string ErrorMessage { get; set; } = string.Empty;

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
