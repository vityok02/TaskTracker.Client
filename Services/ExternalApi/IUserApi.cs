using Domain.Dtos;
using Refit;

namespace Services.ExternalApi;

public interface IUserApi : IApi
{
    [Post("/users/avatar")]
    Task<IApiResponse<FileUrlDto>> UploadAvatarAsync(MultipartFormDataContent avatar);

    [Get("/users")]
    Task<IApiResponse<IEnumerable<UserDto>>> GetUsersAsync();

    [Get("/users/{id}")]
    Task<IApiResponse<UserDto>> GetUserByIdAsync(Guid id);

    [Get("/users")]
    Task<IApiResponse<IEnumerable<UserDto>>> SearchUserByName([Query] string username);
}
