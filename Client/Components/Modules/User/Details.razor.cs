using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Services.ExternalApi;

namespace Client.Components.Modules.User;

[Authorize]
public sealed partial class Details : ComponentBase
{
    [Inject]
    public required IUserApi UserApi { get; init; }

    [Parameter]
    public Guid Id { get; set; }

    protected UserDto User { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var response = await UserApi.GetUserAsync(Id);
        // TODO: Handle response
        User = response.Content;
    }
}