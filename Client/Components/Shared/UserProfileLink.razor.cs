using Domain.Dtos;
using Microsoft.AspNetCore.Components;

namespace Client.Components.Shared;

public partial class UserProfileLink
{
    [Parameter, EditorRequired]
    public required UserInfoDto User { get; set; }

    [Parameter]
    public string? Class { get; set; }

    [Parameter]
    public string? Style { get; set; }

    private string GetClass()
    {
        string defaultClass = "d-flex align-items-center gap-2 text-decoration-none text-dark";

        return Class is null
         ? defaultClass
         : $"{defaultClass} {Class}";
    }
}
