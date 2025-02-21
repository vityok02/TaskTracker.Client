using Domain.Dtos;
using Microsoft.AspNetCore.Components;
using Services.Services.Components;

namespace Client.Components.Modules.User;

public partial class List
{
    [Inject]
    public required IUserService UserService { get; init; }

    public IEnumerable<UserDto> Users { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        var result = await UserService.GetUsersAsync();

        if (result.IsFailure)
        {
            AppState.ErrorMessage = result.Error!.Message;

            return;
        }

        Users = result.Value;
    }
}