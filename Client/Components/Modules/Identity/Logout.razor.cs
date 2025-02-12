using Microsoft.AspNetCore.Components;
using Services.Interfaces.Components;

namespace Client.Components.Modules.Identity;

public sealed partial class Logout : ComponentBase
{
    private readonly IIdentityService _identityService;
    private readonly NavigationManager _navigationManager;

    public Logout(
        NavigationManager navigationManager,
        IIdentityService identityService)
    {
        _navigationManager = navigationManager;
        _identityService = identityService;
    }

    protected override void OnInitialized()
    {
        _identityService.Logout();

        _navigationManager.NavigateTo("/");
    }
}
