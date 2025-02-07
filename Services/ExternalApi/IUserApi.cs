using Domain.Dtos.User;
using Refit;

namespace Services.ExternalApi;

public interface IUserApi : ITaskTrackerApi
{
    [Get("/users")]
    Task<IApiResponse<IEnumerable<UserDto>>> GetUsersAsync();

    [Get("/users/{id}")]
    Task<IApiResponse<UserDto>> GetUserAsync(Guid id);
}
