using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Services.ExternalApi;

namespace Client.Components.Modules.User;

[Authorize]
public sealed partial class Details : ComponentBase
{
    private readonly IUserApi _userApi;

    public Details(IUserApi userApi)
    {
        _userApi = userApi;
    }

    [Parameter]
    public Guid Id { get; set; }

    protected UserDto User { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var response = await _userApi.GetUserAsync(Id);
        User = response.Content;
    }
}