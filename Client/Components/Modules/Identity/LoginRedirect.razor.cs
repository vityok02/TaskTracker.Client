namespace Client.Components.Modules.Identity;

using Microsoft.AspNetCore.Components;

public partial class LoginRedirect : ComponentBase
{
    [Inject]
    public required NavigationManager NavManager { get; init; }

    protected override void OnInitialized()
    {
        NavManager.NavigateTo("/login");
    }
}
