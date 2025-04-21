using Microsoft.AspNetCore.Components;

namespace Client.Components.Shared;

public partial class RoleAuthorizeView
{
    [Parameter] public string[] AllowedRoles { get; set; } = [];

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public RenderFragment? Allowed { get; set; }

    [Parameter] public RenderFragment? NotAllowed { get; set; }

    [CascadingParameter] public string? Role { get; set; }

    private bool? IsAuthorized;

    protected override void OnParametersSet()
    {
        if (Role is null)
        {
            IsAuthorized = false;
            return;
        }

        IsAuthorized = AllowedRoles
            .Contains(Role, StringComparer.OrdinalIgnoreCase);
    }
}
