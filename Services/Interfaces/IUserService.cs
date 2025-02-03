using Domain.Dtos.User;
using Refit;

namespace Services.Interfaces;

public interface IUserService : ITaskTrackerApi
{
    [Get("/users")]
    Task<IEnumerable<UserDto>> GetUsersAsync();

    [Get("/users/{id}")]
    Task<UserDto> GetUserAsync(Guid id);
}
