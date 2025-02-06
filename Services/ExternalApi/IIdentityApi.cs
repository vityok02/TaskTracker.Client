using Domain.Models.Identity;
using Domain.Responses;
using Refit;

namespace Services.ExternalApi;

public interface IIdentityApi : ITaskTrackerApi
{
    [Post("/users/register")]
    Task<IApiResponse<RegisterResponse>> RegisterAsync(RegisterModel model);

    [Post("/users/login")]
    Task<IApiResponse<TokenResponse>> LoginAsync(LoginModel model);
}