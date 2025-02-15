using Domain.Dtos;
using Microsoft.AspNetCore.Components;
using Services.ExternalApi;

namespace Client.Components.Modules.User;

public partial class List
{
    [Inject]
    public required IUserApi UserService { get; init; }

    public List(IUserApi userService)
    {
        UserService = userService;
    }

    public IEnumerable<UserDto> Users { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        var response = await UserService.GetUsersAsync();

        // TODO: Handle response

        Users = response.Content;
    }
}