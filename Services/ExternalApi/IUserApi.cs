using Domain.Dtos;
using Refit;

namespace Services.ExternalApi;

public interface IUserApi : IApi
{
    [Get("/users")]
    Task<IApiResponse<IEnumerable<UserDto>>> GetUsersAsync();

    [Get("/users/{id}")]
    Task<IApiResponse<UserDto>> GetUserAsync(Guid id);
}
