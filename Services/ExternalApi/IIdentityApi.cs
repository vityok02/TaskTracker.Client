using Domain.Dtos;
using Domain.Models.Identity;
using Refit;

namespace Services.ExternalApi;

public interface IIdentityApi : ITaskTrackerApi
{
    [Post("/users/register")]
    Task<IApiResponse<RegisterDto>> RegisterAsync(RegisterModel model);

    [Post("/users/login")]
    Task<IApiResponse<TokenDto>> LoginAsync(LoginModel model);
}