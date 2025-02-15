using Microsoft.AspNetCore.Components;
using Services.Interfaces.Components;

namespace Client.Components.Modules.Identity;

public sealed partial class Logout : ComponentBase
{
    [Inject]
    public required IIdentityService IdentityService { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    protected override void OnInitialized()
    {
        IdentityService.Logout();

        NavigationManager.NavigateTo("/");
    }
}
