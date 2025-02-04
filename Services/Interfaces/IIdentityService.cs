using Domain.Models.Identity;
using Domain.Responses;
using Refit;

namespace Services.Interfaces;

public interface IIdentityService : ITaskTrackerApi
{
    [Post("/users/register")]
    Task<ApiResponse<RegisterResponse>> RegisterAsync(RegisterModel model);

    [Post("/users/login")]
    Task<TokenResponse> LoginAsync(LoginModel model);
}