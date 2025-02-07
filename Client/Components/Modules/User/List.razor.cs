using Domain.Dtos.User;
using Services.ExternalApi;

namespace Client.Components.Modules.User;

public partial class List
{
    private readonly IUserApi _userService;

    public List(IUserApi userService)
    {
        _userService = userService;
    }

    public IEnumerable<UserDto> Users { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        var response = await _userService.GetUsersAsync();

        Users = response.Content;
    }
}