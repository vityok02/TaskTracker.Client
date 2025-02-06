using Domain.Dtos.User;
using Refit;

namespace Services.ExternalApi;

public interface IUserService : ITaskTrackerApi
{
    [Get("/users")]
    Task<ApiResponse<IEnumerable<UserDto>>> GetUsersAsync();

    [Get("/users/{id}")]
    Task<ApiResponse<UserDto>> GetUserAsync(Guid id);
}
