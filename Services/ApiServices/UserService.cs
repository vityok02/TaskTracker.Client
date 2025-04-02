using Domain.Abstract;
using Domain.Dtos;
using Services.Extensions;
using Services.ExternalApi;
using Services.Interfaces.ApiServices;

namespace Services.ApiServices;

public class UserService : IUserService
{
    private readonly IUserApi _userApi;

    public UserService(IUserApi userApi)
    {
        _userApi = userApi;
    }

    public async Task<Result<FileUrlDto>> UploadAvatarAsync(MultipartFormDataContent avatar)
    {
        var response = await _userApi
            .UploadAvatarAsync(avatar);

        return response
            .HandleResponse();
    }

    public async Task<Result<UserDto>> GetUserByIdAsync(Guid userId)
    {
        var response = await _userApi
            .GetUserByIdAsync(userId);

        return response
            .HandleResponse();
    }

    public async Task<Result<IEnumerable<UserDto>>> GetUsersAsync()
    {
        var response = await _userApi
            .GetUsersAsync();

        return response.HandleResponse();
    }

    public async Task<Result<IEnumerable<UserDto>>> SearchUsersAsync(string username)
    {
        var response = await _userApi
            .SearchUserByName(username);

        return response.HandleResponse();
    }
}
